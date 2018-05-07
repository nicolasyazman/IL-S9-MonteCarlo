using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public class MeetingInstance : SolutionInstance
    {
        public MeetingInstance( Meeting m, IReadOnlyList<int> coordinates )
            : base( m, coordinates )
        {
        }

        public new Meeting Space => (Meeting)base.Space;

        SimpleFlight GetArrivalFligth( int idxGuest )
        {
            return Space.Guests[idxGuest].ArrivalFlights[Coordinates[idxGuest]];
        }

        SimpleFlight GetDepartureFligth( int idxGuest )
        {
            return Space.Guests[idxGuest].DepartureFlights[Coordinates[idxGuest+Space.Guests.Count]];
        }

        protected override double ComputeCost()
        {
            double totalCost = 0.0;
            DateTime lastArrivalTime = DateTime.MinValue;
            DateTime firstDepartureTime = DateTime.MaxValue;
            for( int i = 0; i < Space.Guests.Count; ++i )
            {
                var arrival = GetArrivalFligth( i );
                var departure = GetDepartureFligth( i );
                if( arrival.ArrivalTime > lastArrivalTime ) lastArrivalTime = arrival.ArrivalTime;
                if( departure.DepartureTime < firstDepartureTime ) firstDepartureTime = departure.DepartureTime;
            }
            for( int i = 0; i < Space.Guests.Count; ++i )
            {
                var guest = Space.Guests[i];
                var arrival = GetArrivalFligth( i );
                var departure = GetDepartureFligth( i );
                // Not a good idea: this is done by filtering flights during intialization.
                //if( guest.NoStop && (arrival.Stops > 0 || departure.Stops > 0) )
                //{
                //    return Double.MaxValue;
                //}
                totalCost += arrival.Price + departure.Price;
                TimeSpan waitingTimeA = lastArrivalTime - arrival.ArrivalTime;
                TimeSpan waitingTimeD = departure.DepartureTime - firstDepartureTime;
                Debug.Assert( waitingTimeA >= TimeSpan.Zero && waitingTimeD >= TimeSpan.Zero );

                totalCost += (waitingTimeA.TotalMinutes + waitingTimeD.TotalMinutes) * (guest.IsVIP ? 4 : 2);
            }

            return totalCost;
        }

        protected override IEnumerable<SolutionInstance> GetNeighbors()
        {
            return Coordinates.Select<int, SolutionInstance>( (coord, i) =>
            {
                if( coord + 1 < Space.Dimensions[i] )
                {
                    int[] neighborCoordinates = Coordinates.ToArray();
                    neighborCoordinates[i] = coord + 1;
                    return Space.CreateInstance( neighborCoordinates );
                }
                return null;
            } ).Concat(Coordinates.Select<int, SolutionInstance>((coord, i) => {
                 if (coord - 1 >= 0)
                {
                    int[] neighborCoordinates = Coordinates.ToArray();
                    neighborCoordinates[i] = coord - 1;
                    return Space.CreateInstance( neighborCoordinates );
                }
                return null;
            } )).Where((neighbor) => neighbor != null);
            
        }
    }
}
