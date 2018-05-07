using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algo.Optim
{
    public class Meeting : SolutionSpace
    {
        public Meeting( int randomSeed, FlightDatabase db )
            : base( randomSeed )
        {
            FlightDatabase = db;
            Location = Airport.FindByCode( "LHR" );
            MaxBusTimeOnArrival = new DateTime( 2010, 7, 27, 17, 0, 0 );
            MinBusTimeOnDeparture = new DateTime( 2010, 8, 3, 15, 0, 0 );
            Guests = new[]
            {
                new Guest( this, "Kaï", Airport.FindByCode( "BER" ) ),
                new Guest( this, "Erwan", Airport.FindByCode( "CDG" ) ),
                new Guest( this, "Robert", Airport.FindByCode( "MRS" ) ),
                new Guest( this, "Paul", Airport.FindByCode( "LYS" ) ),
                new Guest( this, "James", Airport.FindByCode( "MAN" ) ),
                new Guest( this, "Pedro", Airport.FindByCode( "BIO" ) ),
                new Guest( this, "John", Airport.FindByCode( "JFK" ) ),
                new Guest( this, "Abdel", Airport.FindByCode( "TUN" ) ),
                new Guest( this, "Isabella", Airport.FindByCode( "MXP" ) )
            };
            // Initialize 
            int[] spaceDimensions = new int[2 * Guests.Count];
            int i = 0;
            foreach( var g in Guests )
            {
                spaceDimensions[i] = g.ArrivalFlights.Count;
                spaceDimensions[i + Guests.Count] = g.DepartureFlights.Count;
                ++i;
            }
            Initialize( spaceDimensions );
        }

        public FlightDatabase FlightDatabase { get; }

        public Airport Location { get; }

        public DateTime MaxBusTimeOnArrival { get; }

        public DateTime MinBusTimeOnDeparture { get; }

        public IReadOnlyList<Guest> Guests { get; }

        protected override SolutionInstance DoCreateInstance( IReadOnlyList<int> coordinates )
        {
            return new MeetingInstance( this, coordinates );
        }
    }
}
