using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public abstract class Solver
    {
        protected SolutionSpace _space;

        protected Solver(SolutionSpace space)
        {
            _space = space;
        }
        

        /// <summary>
        /// Solves the problem associated with the solution space.
        /// </summary>
        /// <returns></returns>
        public SolutionInstance Solve(Func<SolutionInstance, IEnumerable<SolutionInstance>, bool> endCondition, int populationSizePerIteration, int maxIterations)
        {
            SolutionInstance best = null;

            IEnumerable<SolutionInstance> population = null;

            int currentIteration = 0;
            while (!endCondition(best, population) && currentIteration < maxIterations )
            {
                population = GeneratePopulation(population, populationSizePerIteration );
                population = FindBestIndividualsInPopulation(population);
                best = _space.TheBest;
                currentIteration++;
            }
            return best;
        }

        public abstract IEnumerable<SolutionInstance> GeneratePopulation( IEnumerable<SolutionInstance> previousPopulation,  int populationSize);

        public abstract IEnumerable<SolutionInstance> FindBestIndividualsInPopulation(IEnumerable<SolutionInstance> population);
    }
}
