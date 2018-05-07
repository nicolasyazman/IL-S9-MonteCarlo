
namespace Algo
{
    public struct WeightedMovie
    {
        public readonly Movie Movie;
        public readonly double TotalWeight;
        public readonly int Count;

        public double Weight => TotalWeight / Count;

        public WeightedMovie(Movie m, double w, int count)
        {
            Movie = m;
            TotalWeight = w;
            Count = count;
        }
    }
}

