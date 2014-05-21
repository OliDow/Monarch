using System;
using MonarchTestBooking.Models.BaseClasses;

namespace MonarchTestBooking.Models
{
    public class Flight : BaseModel<Guid>
    {
        public string FlightNumber { get; set; }

        public string DepartureAirport { get; set; }
        public string DepartureAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public string ArrivalAirport { get; set; }
        public string ArrivalAirportCode { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
