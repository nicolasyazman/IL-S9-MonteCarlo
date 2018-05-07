using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    public class UserRecoOptions
    {
        public int NbSimilarUser { get; set; } = 500;

        public int DeltaMovieRating { get; set; } = 3;

        public int MaxMovieCount { get; set; } = 50;

        public bool OnlyPositiveUserSimilarity { get; set; }

        public override string ToString() => $"NbSimilarUser: {NbSimilarUser}, DeltaMovieRatio: {DeltaMovieRating}, MaxMovieCount: {MaxMovieCount}, OnlyPositiveUserSimilarity: {OnlyPositiveUserSimilarity}";

    }

}
