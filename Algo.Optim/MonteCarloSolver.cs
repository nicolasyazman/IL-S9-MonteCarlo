using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public class MonteCarloSolver : Solver
    {

        public MonteCarloSolver(SolutionSpace space) : base( space )
        {

        }

        public override IEnumerable<SolutionInstance> FindBestIndividualsInPopulation( IEnumerable<SolutionInstance> population )
        {
            double threshold = population.Average( ( individual ) => individual.Cost );
            return population.Select( ( individual ) => {
                SolutionInstance bestNeighbor = individual.BestAmongNeigbors();
                if( bestNeighbor.Cost < individual.Cost )
                {
                    return bestNeighbor;
                }
                else
                {
                    // Instead of only returning the individual, let's create a whole new individual to give us more diversity
                    return _space.GetRandomInstance();
                }
            } ).Where( ( individual ) => individual.Cost <= threshold ).ToArray();
            
        }

        public override IEnumerable<SolutionInstance> GeneratePopulation( IEnumerable<SolutionInstance> previousPopulation, int populationSize )
        {
            List<SolutionInstance> newPopulation = new List<SolutionInstance>();

            if (previousPopulation == null)
            {
                for (int i = 0; i < populationSize; i++ )
                {
                    newPopulation.Add( _space.GetRandomInstance() );
                }
                return newPopulation;
            }

            newPopulation = previousPopulation.ToList();
            int newPopulationSize = newPopulation.Count;
            while (newPopulationSize < populationSize)
            {
                newPopulation.Add( _space.GetRandomInstance() );
                newPopulationSize++;
            }
           return newPopulation;
        }
    }
}
