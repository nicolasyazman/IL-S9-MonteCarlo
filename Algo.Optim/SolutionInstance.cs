using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public abstract class SolutionInstance
    {
        readonly SolutionSpace _space;
        readonly IReadOnlyList<int> _coordinates;
        double _cost;
        IEnumerable<SolutionInstance> _neighbors;

        protected SolutionInstance( SolutionSpace space, IReadOnlyList<int> coordinates )
        {
            _space = space;
            _coordinates = coordinates;
            _cost = -1.0;
            _neighbors = null;
        }

        public SolutionSpace Space => _space;

        public IEnumerable<SolutionInstance> Neighbors
        {
            get
            {
                if( _neighbors == null )
                {
                    _neighbors = GetNeighbors();
                }
                return _neighbors;
            }
        }

        /// <summary>
        /// Gets the minimal cost among <see cref="Neighbors"/>.
        /// Never null since we always have at least one neighbor.
        /// </summary>
        /// <returns>The best neighbor.</returns>
        public SolutionInstance BestAmongNeigbors()
        {
            return Neighbors.Aggregate( ( curMin, x ) => (curMin == null || (x.Cost) < curMin.Cost ? x : curMin) );
        }

        public IReadOnlyList<int> Coordinates => _coordinates;

        public double Cost
        {
            get
            {
                if( _cost < 0 ) _cost = ComputeCost();
                return _cost;
            }

        }

        protected abstract double ComputeCost();

        protected abstract IEnumerable<SolutionInstance> GetNeighbors();
    }
}
