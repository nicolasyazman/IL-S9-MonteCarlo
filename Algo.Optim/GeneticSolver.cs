using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{

    public static class Extend
    {
        public static double StandardDeviation( this IEnumerable<int> values )
        {
            double avg = values.Average();
            return Math.Sqrt( values.Average( v => Math.Pow( v - avg, 2 ) ) );
        }

        public static double Norm( this IEnumerable<double> values )
        {
            double Norm = Math.Sqrt( values.Sum() );
            return Norm;
        }
    }

    public class GeneticSolver : Solver
    {
        bool _introduceDiversity;

        // diversityFactor should be between 0 and 1. 0 means bringing no diversity, 1 means everything should be diverse 
        double _diversityFactor;
        double _diversityThreshold; 

        public GeneticSolver(SolutionSpace space, bool introduceDiversity = false, double diversityFactor = 0.5, double diversityThreshold = 30) : base(space)
        {
            _introduceDiversity = introduceDiversity;
            _diversityFactor = diversityFactor;
            _diversityThreshold = diversityThreshold;
        }


        public override IEnumerable<SolutionInstance> FindBestIndividualsInPopulation( IEnumerable<SolutionInstance> population )
        {
            double threshold = population.Average( ( individual ) => individual.Cost );
            return population.Where( ( individual ) => individual.Cost <= threshold );
        }

        public override IEnumerable<SolutionInstance> GeneratePopulation(IEnumerable<SolutionInstance> previousPopulation, int populationSize)
        {
            List<SolutionInstance> newPopulation = new List<SolutionInstance>();
            if (previousPopulation == null)
            {
                for( int i = 0; i < populationSize; i++ )
                {
                    SolutionInstance randomInstance = _space.GetRandomInstance();
                    newPopulation.Add( randomInstance );
                }
                return newPopulation;
            }

            // Arbitrary number
            int breakingIndex = _space.Random.Next(1,_space.Dimensions.Count()-1);

            int breedingsDone;
            if (_introduceDiversity && ComputeDiversity(previousPopulation) < _diversityThreshold)
            {
                breedingsDone = (int)Math.Round( (populationSize / 2 * (1 - _diversityFactor)));
            }
            else
            {
                breedingsDone = populationSize / 2;
            }

            // Each crossover brings two new children
            for (int i = 0; i < breedingsDone; i++ )
            {
                SolutionInstance[] parents = previousPopulation.Skip(_space.Random.Next(previousPopulation.Count()-1)).Take( 2 ).ToArray();
                SolutionInstance father = parents[0];
                SolutionInstance mother = parents[1];

                int[] childOneCoordinates = father.Coordinates.Take( breakingIndex ).Concat( mother.Coordinates.Skip( breakingIndex ).Take( _space.Dimensions.Count() - breakingIndex ) ).ToArray();
                int[] childTwoCoordinates = mother.Coordinates.Take( breakingIndex ).Concat( father.Coordinates.Skip( breakingIndex ).Take( _space.Dimensions.Count() - breakingIndex ) ).ToArray();
                
                SolutionInstance childOne = _space.CreateInstance( childOneCoordinates );
                SolutionInstance childTwo = _space.CreateInstance( childTwoCoordinates );
                newPopulation.Add( childOne );
                newPopulation.Add( childTwo );
            }

            int newPopulationSize = newPopulation.Count();
            while (newPopulationSize < populationSize)
            {
                newPopulation.Add(_space.GetRandomInstance());
                newPopulationSize++;
            }
            return newPopulation;
        }

        double ComputeDiversity(IEnumerable<SolutionInstance> population)
        {
            int populationSize = population.Count();

            double[] centralPoint = new double[_space.Dimensions.Count()];

            foreach (SolutionInstance individual in population)
            {
                for (int i = 0; i < individual.Coordinates.Count();i++ )
                {
                    centralPoint[i] = centralPoint[i] + individual.Coordinates[i];
                }
            }

            centralPoint = centralPoint.Select( coord => coord / populationSize ).ToArray();

            double dispersion = 0;
            foreach (SolutionInstance individual in population)
            {
                dispersion += individual.Coordinates.Zip( centralPoint, ( one, two ) => Math.Pow(one - two,2) ).Norm();
            }
            dispersion /= populationSize;

            return dispersion;
        }
    }
}
