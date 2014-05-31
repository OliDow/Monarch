using System.Collections.Generic;
using MonarchTestBooking.Models;

namespace MonarchTestBooking.Data.Interface
{
    public interface IBookingService
    {
        List<Flight> GetAllFlights();
        List<Flight> SearchFlights(BookingSearchParams paramaters);
        Flight GetFlight(string flightName);

        bool BookSeat(string flightNumber);
        bool UpdateStatus(string flightNumber, FlightStatus status);
    }

}
