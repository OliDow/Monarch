using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MonarchTestBooking.Data;
using MonarchTestBooking.Data.Service;
using MonarchTestBooking.Models;
using MonarchTestBooking.ViewModels.Booking;

namespace MonarchTestBooking.Controllers
{
    public class BookingController : Controller
    {
        //
        // GET: /Booking/
        public ActionResult Index()
        {
            // TODO Setup some sort of DI and inject this, to remove EF reference and east testing
            MonarchContext context = new MonarchContext();
            BookingService service = new BookingService(context);

            var vm = new IndexViewModel();
            vm.FullFlightList = service.GetAllFlights();

            BookingSearchParams bsp = new BookingSearchParams
            {
                ArrivalAirportCode = "LAX"
            };

            vm.SearchResults = service.SearchFlights(bsp);

            return View(vm);
        }

        // TODO Ajax this when we have some spare time and pass a message back
        public ActionResult CancelFlight(string flightNumber)
        {
            // TODO Setup some sort of DI and inject this, to remove EF reference and east testing
            MonarchContext context = new MonarchContext();
            BookingService service = new BookingService(context);

            var vm = new CancellationViewModel();
            Flight flight = service.GetFlight(flightNumber);

            // Display error if the flight Can#t be found
            if (flight == null)
            {
                vm.Message = "Flight not found";
            }

            // Check if the flight is already Cancelled
            else if (flight.FlightStatus == FlightStatus.Cancelled)
            {
                //TODO Add Logger and log exception
                vm.Message = string.Format("Flight {0} is already cancelled", flight.FlightNumber);
            }
            else
            {
                var cancelled = service.UpdateStatus(flightNumber, FlightStatus.Cancelled);
                if (cancelled)
                {
                    vm.Success = true;
                    vm.Message = string.Format("Flight Number {0} has been cancelled", flight.FlightNumber);
                }
                else
                {
                    //TODO Add Logger and log exception
                    vm.Message = string.Format("Error cancelling Flight Number {0}", flight.FlightNumber);
                }
            }

            return View(vm);
        }

        public ActionResult BookSeat(string flightNumber)
        {
            // TODO Setup some sort of DI and inject this, to remove EF reference and east testing
            MonarchContext context = new MonarchContext();
            BookingService service = new BookingService(context);

            var vm = new SeatBookedViewModel();
            Flight flight = service.GetFlight(flightNumber);

            // Display error if the flight Can#t be found
            if (flight == null)
            {
                vm.Message = "Flight not found";
            }
            else if (flight.SeatsBooked >= flight.SeatsOnFlight)
            {
                vm.Message = string.Format("Flight {0} is already at capacity", flight.FlightNumber);
            }
            else
            {
                var booked = service.BookSeat(flightNumber);
                if (booked)
                {
                    vm.Message = string.Format("A seat on Flight Number {0} has been booked successfully",flight.FlightNumber);
                    vm.Success = true;
                }
                else
                {
                    //TODO Add Logger and log exception
                    vm.Message = string.Format("Error booking a seat on Flight Number {0}", flight.FlightNumber);
                }
            }

            return View(vm);
        }
    }
}