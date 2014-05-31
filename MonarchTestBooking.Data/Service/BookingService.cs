using System;
using System.Collections.Generic;
using System.Linq;
using MonarchTestBooking.Data.Interface;
using MonarchTestBooking.Models;

namespace MonarchTestBooking.Data.Service
{
    public class BookingService : IBookingService
    {
        private readonly MonarchContext _context;

        public BookingService(MonarchContext context)
        {
            _context = context;
        }

        public List<Flight> GetAllFlights()
        {
            var query = _context.Flights;

            return query.ToList();
        }

        public List<Flight> SearchFlights(BookingSearchParams parameters)
        {
            IQueryable<Flight> query = _context.Flights.OrderBy(f => f.DepartureTime);

            //TODO Impliment this if a sort by options is given to the user
            //  if (paramaters.OrderByDepartureTime)
            //      query = query.OrderBy(f => f.DepartureTime);

            if (!parameters.IncludeFlightsAtCapacity)
                query = query.Where(f => f.SeatsBooked < f.SeatsOnFlight);

            if (!string.IsNullOrEmpty(parameters.ArrivalAirportCode))
                query = query.Where(f => f.ArrivalAirportCode == parameters.ArrivalAirportCode);

            return query.ToList();
        }

        public bool BookSeat(string flightNumber)
        {
            var flight = this.GetFlight(flightNumber);
            
            // Return false if flight not found
            if (flight == null)
            {
                //TODO Add Logger and log exception
                return false;
            }

            // Double check this update is allowed and the flight is not at capacity
            if (flight.SeatsBooked >= flight.SeatsOnFlight)
            {
                //TODO Add Logger and log exception
                return false;
            }

            flight.SeatsBooked++;

            // check the seat was booked succesfully
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO Add Logger and log exception
                return false;
            }

            return true;
        }

        public bool UpdateStatus(string flightNumber, FlightStatus status)
        {
            var flight = this.GetFlight(flightNumber); //_context.Flights.FirstOrDefault(f => f.FlightNumber == flightNumber));

            // Return false if flight not found
            if (flight == null)
            {
                //TODO Add Logger and log exception
                return false;
            }
            
            flight.FlightStatus = FlightStatus.Cancelled;
            
            // check the flught was cancelled succesfully
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO Add Logger and log exception
                return false;
            }

            return true;

        }


        public Flight GetFlight(string flightNumber)
        {
            return _context.Flights.FirstOrDefault(f => f.FlightNumber == flightNumber);
        }
    }
}
