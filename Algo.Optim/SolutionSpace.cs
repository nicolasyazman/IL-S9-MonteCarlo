using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public abstract class SolutionSpace
    {
        readonly Random _random;
        SolutionInstance _theBest;

        public SolutionSpace( int randomSeed )
        {
            _random = new Random( randomSeed );
        }

        public IReadOnlyList<int> Dimensions { get; private set; }

        public double Cardinality => Dimensions.Aggregate( 1, ( acc, value ) => acc * value );

        public SolutionInstance TheBest => _theBest;

        public SolutionInstance GetRandomInstance()
        {
            var randomCoordinates = Dimensions.Select( count => _random.Next( count ) ).ToArray();
            return CreateInstance( randomCoordinates );
        }

        public SolutionInstance CreateInstance( IReadOnlyList<int> coordinates )
        {
            var s = DoCreateInstance( coordinates );
            if( _theBest == null || s.Cost < _theBest.Cost ) _theBest = s;
            return s;
        }

        protected abstract SolutionInstance DoCreateInstance( IReadOnlyList<int> coordinates );

        protected void Initialize( IReadOnlyList<int> spaceDimensions )
        {
            if( spaceDimensions == null ) throw new ArgumentNullException( nameof(spaceDimensions) );
            Dimensions = spaceDimensions;
        }

        public Random Random => _random;
    }
}
