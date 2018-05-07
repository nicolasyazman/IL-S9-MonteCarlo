using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public class SimulatedAnnealingSolver : Solver
    {
        public SimulatedAnnealingSolver(SolutionSpace space) : base(space)
        {

        }

        public override IEnumerable<SolutionInstance> FindBestIndividualsInPopulation( IEnumerable<SolutionInstance> population )
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<SolutionInstance> GeneratePopulation( IEnumerable<SolutionInstance> previousPopulation, int populationSize )
        {
            List<SolutionInstance> newPopulation = new List<SolutionInstance>();

            if( previousPopulation == null )
            {
                for( int i = 0; i < populationSize; i++ )
                {
                    newPopulation.Add( _space.GetRandomInstance() );
                }
                return newPopulation;
            }



            return previousPopulation;
        }
    }
}
