using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Algo
{
    public class RecoContext
    {
        public IReadOnlyList<User> Users { get; private set; }
        public IReadOnlyList<Movie> Movies { get; private set; }
        public int RatingCount { get; private set; }

        public bool LoadFrom( string folder )
        {
            string p = Path.Combine( folder, "users.dat" );
            if( !File.Exists( p ) ) return false;
            Users = User.ReadUsers( p );
            p = Path.Combine( folder, "movies.dat" );
            if( !File.Exists( p ) ) return false;
            Movies = Movie.ReadMovies( p );
            p = Path.Combine( folder, "ratings.dat" );
            if( !File.Exists( p ) ) return false;
            RatingCount = User.ReadRatings( Users, Movies, p );
            return true;
        }

        static public double DNorm2( User u1, User u2 )
        {
            var squareDeltas = u1.Ratings.Select( mr1 => new
            {
                R1 = mr1.Value,
                R2 = u2.Ratings.GetValueWithDefault( mr1.Key, -1 )
            } )
            .Where( r1r2 => r1r2.R2 >= 0 )
            .Select( r1r2 => r1r2.R1 - r1r2.R2 )
            .Select( delta => delta * delta );
            // By considering the users as being the same, we boost
            // the movies loved the most by all users.
            return squareDeltas.Any()
                    ? Math.Sqrt( squareDeltas.Sum() )
                    : 0.0;
        }

        static public double SimilarityPearson( User u1, User u2 )
        {
            IEnumerable<Movie> common = u1.Ratings.Keys.Intersect( u2.Ratings.Keys );
            return SimilarityPearson( common.Select( m => new KeyValuePair<int, int>( u1.Ratings[m], u2.Ratings[m] ) ) );
        }

        static public double SimilarityPearson( IEnumerable<KeyValuePair<int, int>> values )
        {
            double sumX = 0.0;
            double sumY = 0.0;
            double sumXY = 0.0;
            double sumX2 = 0.0;
            double sumY2 = 0.0;

            int count = 0;
            foreach( var m in values )
            {
                count++;
                int x = m.Key;
                int y = m.Value;
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
                sumY2 += y * y;
            }
            if( count == 0 ) return 0.0;
            if( count == 1 )
            {
                var onlyOne = values.Single();
                double d = Math.Abs( onlyOne.Key - onlyOne.Value );
                return 1 / (1 + d);
            }
            double numerator = sumXY - (sumX * sumY / count);
            double denumerator1 = sumX2 - (sumX * sumX / count);
            double denumerator2 = sumY2 - (sumY * sumY / count);
            var result = numerator / Math.Sqrt( denumerator1 * denumerator2 );
            if( double.IsNaN( result ) )
            {
                double sumSquare = values.Select( v => v.Key - v.Value ).Select( v => v * v ).Sum();
                result = 1.0 / (1 + Math.Sqrt( sumSquare ));
            }
            return result;
        }

        public IReadOnlyList<SimilarUser> GetSimilarUsers(
            User u,
            int nbSimilarUser,
            bool onlyPositiveUserSimilarity )
        {
            var best = onlyPositiveUserSimilarity
                       ? new BestKeeper<SimilarUser>( nbSimilarUser,
                            ( s1, s2 ) => Math.Sign( s2.Similarity - s1.Similarity ) )
                       : new BestKeeper<SimilarUser>( nbSimilarUser,
                            ( s1, s2 ) => Math.Sign( Math.Abs( s2.Similarity ) - Math.Abs( s1.Similarity ) ) );

            foreach( var other in Users )
            {
                if( other == u ) continue;
                SimilarUser sU = new SimilarUser( other, SimilarityPearson( u, other ) );
                best.Add( sU );
            }
            return best;
        }

        public IReadOnlyList<WeightedMovie> GetRecoMovies( User u, UserRecoOptions options )
        {
            var users = GetSimilarUsers( u, options.NbSimilarUser, options.OnlyPositiveUserSimilarity );
            var allMovies = new Dictionary<Movie, WeightedMovie>();
            foreach( var other in users )
            {
                foreach( var movieRated in other.User.Ratings )
                {
                    var m = movieRated.Key;
                    if( u.Ratings.ContainsKey( m ) ) continue;
                    allMovies.TryGetValue( m, out WeightedMovie already );
                    double deltaWeight = other.Similarity
                                            * (movieRated.Value - options.DeltaMovieRating);
                    allMovies[m] = new WeightedMovie(
                                        m,
                                        already.TotalWeight + deltaWeight,
                                        already.Count + 1 );
                }
            }
            var bestMovies = new BestKeeper<WeightedMovie>(
                                    options.MaxMovieCount,
                                    ( m1, m2 ) => Math.Sign( m2.Weight - m1.Weight ) );
            foreach( var mW in allMovies.Values ) bestMovies.Add( mW );
            return bestMovies;
        }
    }
}
