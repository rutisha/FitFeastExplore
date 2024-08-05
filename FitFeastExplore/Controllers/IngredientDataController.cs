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
    public class IngredientDataController : ApiController
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
        public List<IngredientDto> ListIngredients()
        {
            List<Ingredient> Ingredients = db.Ingredients.ToList();
            List<IngredientDto> IngredientDtos = new List<IngredientDto>();

            Ingredients.ForEach(i => IngredientDtos.Add(new IngredientDto()
            {
                IngredientId = i.IngredientId,
                IngredientName = i.IngredientName,
                Quantity = i.Quantity
            }));


            return IngredientDtos;
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
        [ResponseType(typeof(IngredientDto))]
        [HttpGet]
        [Route("api/IngredientData/FindIngredient/{id}")]
        public IHttpActionResult FindIngredient(int id)
        {
            Ingredient Ingredient = db.Ingredients.Find(id);

            if (Ingredient == null)
            {
                return NotFound();
            }

            IngredientDto IngredientDtos = new IngredientDto()
            {
                IngredientId = Ingredient.IngredientId,
                IngredientName = Ingredient.IngredientName,
                Quantity = Ingredient.Quantity
            };

            return Ok(IngredientDtos);
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

        [ResponseType(typeof(Ingredient))]
        [HttpPost]
        [Route("api/IngredientData/AddIngredient")]
        public IHttpActionResult AddIngredient(Ingredient Ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ingredients.Add(Ingredient);
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

        [ResponseType(typeof(Ingredient))]
        [HttpPost]
        [Route("api/IngredientData/DeleteIngredient/{id}")]
        public IHttpActionResult DeleteIngredient(int id)
        {
            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            db.Ingredients.Remove(ingredient);
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
        [Route("api/IngredientData/UpdateIngredient/{id}")]
        public IHttpActionResult UpdateIngredient(int id, Ingredient ingredient)
        {
            Debug.WriteLine("I have reached the update ingredient method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != ingredient.IngredientId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + ingredient.IngredientId);
                Debug.WriteLine("POST parameter" + ingredient.IngredientName);
                Debug.WriteLine("POST parameter " + ingredient.Quantity);
                return BadRequest();
            }

            db.Entry(ingredient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
                {
                    Debug.WriteLine("Ingredient not found");
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
        [Route("api/IngredientData/SearchIngredients")]
        public List<IngredientDto> SearchIngredients(string searchString)
        {
            var ingredients = db.Ingredients
                .Where(i => i.IngredientName.Contains(searchString))
                .ToList();

            return ingredients.Select(i => new IngredientDto
            {
                IngredientId = i.IngredientId,
                IngredientName = i.IngredientName,
                Quantity = i.Quantity
            }).ToList();
        }
        private bool IngredientExists(int id)
        {
            return db.Ingredients.Count(e => e.IngredientId == id) > 0;
        }
    }
}
