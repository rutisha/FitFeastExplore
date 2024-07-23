using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FitFeastExplore.Models;
using System.Web.Http.Description;

namespace FitFeastExplore.Controllers
{
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all bookings in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all bookings in the database.
        /// </returns>
        /// <example>
        /// GET: api/Bookingdata/ListBookings
        /// </example>

        [HttpGet]
        public List<BookingDto> ListBookings()
        {
            List<Booking> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(b => BookingDtos.Add(new BookingDto()
            {
                BookingId = b.BookingId,
                Bookingdate = b.Bookingdate,
                Status = b.Status,
                CustomerName = b.Customer.CustomerName,
                TourName = b.Tour.Tourname
            }));


            return BookingDtos;
        }

        /// <summary>
        /// Gathers information about tour information related to a particular booking
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: specific booking in the database.
        /// </returns>
        /// <param name="id">Booking ID.</param>
        /// <example>
        /// GET: api/Bookingdata/FindBooking/2
        /// </example>
        [ResponseType(typeof(BookingDto))]
        [HttpGet]
        [Route("api/Bookingdata/FindBooking/{id}")]
        public IHttpActionResult FindBooking(int id)
        {
            Booking Booking = db.Bookings.Find(id);

            if (Booking == null)
            {
                return NotFound();
            }

            BookingDto BookingDtos = new BookingDto()
            {
                BookingId = Booking.BookingId,
                Bookingdate = Booking.Bookingdate,
                Status = Booking.Status,
                CustomerName = Booking.Customer.CustomerName,
                TourName = Booking.Tour.Tourname,
                TourId = Booking.Tour.Tourid,
                CustomerId = Booking.Customer.CustomerId

            };

            return Ok(BookingDtos);
        }

        /// <summary>
        /// Adds an new booking to the system
        /// </summary>
        /// <param name="booking">JSON FORM DATA of an booking</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Booking ID, Booking Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// // POST: api/BookingData/AddBooking
        /// FORM DATA: Tour JSON Object
        /// </example>

        [ResponseType(typeof(Booking))]
        [HttpPost]
        [Route("api/BookingData/AddBooking")]
        public IHttpActionResult AddBooking(Booking Booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(Booking);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an booking from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the booking</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/BookingData/DeleteBooking/5
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Booking))]
        [HttpPost]
        [Route("api/BookingData/DeleteBooking/{id}")]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates a particular booking in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Booking ID primary key</param>
        /// <param name="tour">JSON FORM DATA of an booking</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/BookingData/UpdateBooking/5
        /// FORM DATA: Tour JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/BookingData/UpdateBooking/{id}")]
        public IHttpActionResult UpdateBooking(int id, Booking booking)
        {
            Debug.WriteLine("I have reached the update booking method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != booking.BookingId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + booking.BookingId);
                Debug.WriteLine("POST parameter" + booking.Bookingdate);
                Debug.WriteLine("POST parameter " + booking.Status);
                Debug.WriteLine("POST parameter " + booking.CustomerId);
                Debug.WriteLine("POST parameter " + booking.TourId);
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    Debug.WriteLine("Booking not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(BookingDto))]
        [Route("api/BookingData/ListBookingsForCustomer/{id}")]
        public IHttpActionResult ListBookingsForCustomer(int id)
        {
            // SQL Equivalent:
            // Select * from bookings where bookings.customerid = {id}
            List<Booking> bookings = db.Bookings.Where(b => b.CustomerId == id).ToList();
            List<BookingDto> bookingDtos = new List<BookingDto>();

            bookings.ForEach(b => bookingDtos.Add(new BookingDto()
            {
                BookingId = b.BookingId,
                Bookingdate = b.Bookingdate,
                CustomerId = b.Customer.CustomerId,
                CustomerName = b.Customer.CustomerName,
                TourId = b.Tour.Tourid,
                TourName = b.Tour.Tourname
            }));

            return Ok(bookingDtos);
        }

        [HttpGet]
        [ResponseType(typeof(List<BookingDto>))]
        [Route("api/BookingData/ListBookingsForTour/{id}")]
        public IHttpActionResult ListBookingsForTour(int id)
        {
            // SQL Equivalent:
            // Select * from bookings where bookings.tourid = {id}
            List<Booking> bookings = db.Bookings.Where(b => b.TourId == id).ToList();
            List<BookingDto> bookingDtos = new List<BookingDto>();

            bookings.ForEach(b => bookingDtos.Add(new BookingDto()
            {
                BookingId = b.BookingId,
                Bookingdate = b.Bookingdate,
                CustomerId = b.Customer.CustomerId,
                CustomerName = b.Customer.CustomerName,
                TourId = b.Tour.Tourid,
                TourName = b.Tour.Tourname
            }));

            return Ok(bookingDtos);
        }

        /// <summary>
        /// Searches for bookings that match the given search string.
        /// </summary>
        /// <param name="searchString">The search string to filter bookings.</param>
        /// <returns>A list of BookingDto objects that match the search criteria.</returns>
        /// <example>
        /// GET: api/BookingData/SearchBookings?searchString=John =>
        /// <BookingDto>
        /// <BookingId>1</BookingId>
        /// <CustomerId>123</CustomerId>
        /// <TourId>456</TourId>
        /// <BookingDate>2024-06-21</BookingDate>
        /// </BookingDto>
        /// </example>
        [HttpGet]
        [Route("api/BookingData/SearchBookings")]
        public List<BookingDto> SearchBookings(string searchString)
        {
            var bookings = db.Bookings
                .Where(b => b.Customer.CustomerName.Contains(searchString))
                .ToList();

            return bookings.Select(b => new BookingDto
            {
                BookingId = b.BookingId,
                CustomerId = b.CustomerId,
                TourId = b.TourId,
                Bookingdate = b.Bookingdate
                // Add more properties as needed
            }).ToList();
        }
        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingId == id) > 0;
        }
    }
}
