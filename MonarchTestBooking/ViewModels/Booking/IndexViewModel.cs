using System.Collections.Generic;
using MonarchTestBooking.Models;

namespace MonarchTestBooking.ViewModels.Booking
{
    public class IndexViewModel
    {
        public List<Flight> FullFlightList { get; set; }
        public List<Flight> SearchResults { get; set; }
    }
}