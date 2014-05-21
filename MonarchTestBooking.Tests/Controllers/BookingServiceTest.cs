using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MonarchTestBooking.Data;
using MonarchTestBooking.Data.Interface;
using MonarchTestBooking.Data.Service;
using MonarchTestBooking.Models;
using Moq;
using NUnit.Framework;

namespace MonarchTestBooking.Tests.Controllers
{
    [TestFixture]
    public class BookingServiceTest
    {
        private IBookingService _bookingService;

        [SetUp]
        public void Setup()
        {
            var testFlightData = new List<Flight>
           {
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0001",
                   DepartureAirport = "Los Angeles International Airport",
                   DepartureAirportCode = "LAX",
                   DepartureTime = new DateTime(2014, 6, 3, 12, 0, 0),
                   ArrivalAirport = "BCN",
                   ArrivalAirportCode = "Barcelona",
                   ArrivalTime = new DateTime(2014, 6, 3, 20, 0, 0),
               },
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0002",
                   DepartureAirport = "Barcelona",
                   DepartureAirportCode = "BCN",
                   DepartureTime = new DateTime(2014, 6, 2, 2, 0, 0),
                   ArrivalAirport = "Los Angeles International Airport",
                   ArrivalAirportCode = "LAX",
                   ArrivalTime = new DateTime(2014, 6, 2, 9, 0, 0),
               },
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0003",
                   DepartureAirport = "Luton",
                   DepartureAirportCode = "LTN",
                   DepartureTime = new DateTime(2014, 6, 4, 18, 0, 0),
                   ArrivalAirport = "Barcelona",
                   ArrivalAirportCode = "BCN",
                   ArrivalTime = new DateTime(2014, 6, 3, 21, 0, 0),
               },
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0004",
                   DepartureAirport = "Barcelona",
                   DepartureAirportCode = "BCN",
                   DepartureTime = new DateTime(2014, 6, 1, 9, 0, 0),
                   ArrivalAirport = "Luton",
                   ArrivalAirportCode = "LTN",
                   ArrivalTime = new DateTime(2014, 6, 2, 12, 0, 0),
               },
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0005",
                   DepartureAirport = "Los Angeles International Airport",
                   DepartureAirportCode = "LAX",
                   DepartureTime = new DateTime(2014, 6, 3, 12, 30, 0),
                   ArrivalAirport = "Luton",
                   ArrivalAirportCode = "LTN",
                   ArrivalTime = new DateTime(2014, 6, 3, 16, 3, 0),
               },
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0006",
                   DepartureAirport = "Los Angeles International Airport",
                   DepartureAirportCode = "LAX",
                   DepartureTime = new DateTime(2014, 6, 2, 8, 30, 0),
                   ArrivalAirport = "Barcelona",
                   ArrivalAirportCode = "BCN",
                   ArrivalTime = new DateTime(2014, 6, 2, 16, 30, 0),
               },
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0007",
                   DepartureAirport = "Barcelona",
                   DepartureAirportCode = "BCN",
                   DepartureTime = new DateTime(2014, 6, 3, 22, 0, 0),
                   ArrivalAirport = "Luton",
                   ArrivalAirportCode = "LTN",
                   ArrivalTime = new DateTime(2014, 6, 4, 2, 0, 0),
               },
               new Flight
               {
                   Id = new Guid(), 
                   FlightNumber = "0008",
                   DepartureAirport = "Luton",
                   DepartureAirportCode = "LTN",
                   DepartureTime = new DateTime(2014, 6, 3, 12, 0, 0),
                   ArrivalAirport = "Los Angeles International Airport",
                   ArrivalAirportCode = "LAX",
                   ArrivalTime = new DateTime(2014, 6, 3, 20, 0, 0),
               }

           }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Flight>>();
            mockDbSet.As<IQueryable<Flight>>().Setup(m => m.Provider).Returns(testFlightData.Provider);
            mockDbSet.As<IQueryable<Flight>>().Setup(m => m.Expression).Returns(testFlightData.Expression);
            mockDbSet.As<IQueryable<Flight>>().Setup(m => m.ElementType).Returns(testFlightData.ElementType);
            mockDbSet.As<IQueryable<Flight>>().Setup(m => m.GetEnumerator()).Returns(testFlightData.GetEnumerator());

            var mockContext = new Mock<MonarchContext>();
            mockContext.Setup(d => d.Flights).Returns(mockDbSet.Object);

            _bookingService = new BookingService(mockContext.Object);
        }

        [Test]
        public void ListAllFlights()
        {
            var flightList = _bookingService.GetAllFlights();

            Assert.AreEqual(8, flightList.Count);
        }

        [Test]
        public void ListFlights_OrderByDepartureDate()
        {
            var searchParams = new BookingSearchParams();
            var flightList = _bookingService.SearchFlights(searchParams);

            Assert.AreEqual("0002", flightList[1].FlightNumber);
            Assert.AreEqual("0003", flightList[7].FlightNumber);
        }

        [Test]
        public void ListFlights_DestinedForLAX()
        {
            var searchParams = new BookingSearchParams{ ArrivalAirportCode = "LAX"};
            var flightList = _bookingService.SearchFlights(searchParams);

            Assert.AreEqual(2, flightList.Count);
        }

        [Test]
        public void ListFlight_CheckCancellation()
        {
            var searchParams = new BookingSearchParams();

            _bookingService.UpdateStatus("0001", FlightStatus.Cancelled);
            var flightList = _bookingService.SearchFlights(searchParams);
            var cancelledFlightCount = flightList.Count(f => f.FlightStatus == FlightStatus.Cancelled);

            Assert.AreEqual(1, cancelledFlightCount);
        }

    }
}
