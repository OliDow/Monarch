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
	}
}