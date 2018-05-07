using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public class Guest
    {
        public Guest( Meeting m,
                      string name,
                      Airport guestLocation )
        {
            Name = name;
            Location = guestLocation;
            var db = m.FlightDatabase;
            ArrivalFlights = db.GetFlights( m.MaxBusTimeOnArrival, guestLocation, m.Location )
                                .Concat( db.GetFlights( m.MaxBusTimeOnArrival.AddDays( -1 ), guestLocation, m.Location ) )
                                .Where( f => f.ArrivalTime < m.MaxBusTimeOnArrival )
                                .Where( f => f.Stops == 0 || NoStop == false )
                                .OrderByDescending( f => f.ArrivalTime )
                                .ToArray();
            DepartureFlights = db.GetFlights( m.MinBusTimeOnDeparture, m.Location, guestLocation )
                                 .Concat( db.GetFlights( m.MinBusTimeOnDeparture.AddDays( 1 ), m.Location, guestLocation ) )
                                 .Where( f => f.DepartureTime > m.MinBusTimeOnDeparture )
                                 .Where( f => f.Stops == 0 || NoStop == false )
                                 .OrderBy( f => f.DepartureTime )
                                 .ToArray();
        }

        public string Name { get; }

        public Airport Location { get; }

        /// <summary>
        /// No stop is flights required.
        /// (Unused, this is just a sample of constraint.)
        /// </summary>
        public bool NoStop { get; set; }

        /// <summary>
        /// A VIP's minute is twice the price of the regular's one.
        /// </summary>
        public bool IsVIP { get; set; }

        public IReadOnlyList<SimpleFlight> ArrivalFlights { get; }

        public IReadOnlyList<SimpleFlight> DepartureFlights { get; }
    }


}
