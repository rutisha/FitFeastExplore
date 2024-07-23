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
    public class TourDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all tours in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all tours in the database.
        /// </returns>
        /// <example>
        /// GET: api/Tourdata/ListTours
        /// </example>
        [HttpGet]
        public List<TourDto> ListTours()
        {
            List<Tour> Tours = db.Tours.ToList();
            List<TourDto> TourDtos = new List<TourDto>();

            Tours.ForEach(t => TourDtos.Add(new TourDto()
            {
                Tourid = t.Tourid,
                Tourname = t.Tourname,
                Description = t.Description,
                Location = t.Location,
                Price = t.Price
            }));

            return TourDtos;
        }

        /// <summary>
        /// Gathers information about tour information related to a particular tour
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: specific tour in the database.
        /// </returns>
        /// <param name="id">Tour ID.</param>
        /// <example>
        /// GET: api/Tourdata/FindTour/2
        /// </example>

        [ResponseType(typeof(Tour))]
        [HttpGet]
        [Route("api/Tourdata/FindTour/{id}")]
        public IHttpActionResult FindTour(int id)
        {
            Tour Tour = db.Tours.Find(id);

            if (Tour == null)
            {
                return NotFound();
            }

            TourDto TourDtos = new TourDto()
            {
                Tourid = Tour.Tourid,
                Tourname = Tour.Tourname,
                Description = Tour.Description,
                Location = Tour.Location,
                Price = Tour.Price

            };

            return Ok(TourDtos);
        }

        /// <summary>
        /// Adds an new tour to the system
        /// </summary>
        /// <param name="tour">JSON FORM DATA of an tour</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Tour ID, Tour Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// // POST: api/TourData/AddTour
        /// FORM DATA: Tour JSON Object
        /// </example>
        [ResponseType(typeof(Tour))]
        [HttpPost]
        [Route("api/TourData/AddTour")]
        public IHttpActionResult AddTour(Tour Tour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tours.Add(Tour);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an tour from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the tour</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TourData/DeleteTour/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Tour))]
        [HttpPost]
        [Route("api/TourData/DeleteTour/{id}")]
        public IHttpActionResult DeleteTour(int id)
        {
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return NotFound();
            }

            db.Tours.Remove(tour);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates a particular tour in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Tour ID primary key</param>
        /// <param name="tour">JSON FORM DATA of an tour</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TourData/UpdateTour/5
        /// FORM DATA: Tour JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/TourData/UpdateTour/{id}")]
        public IHttpActionResult UpdateTour(int id, Tour tour)
        {
            Debug.WriteLine("I have reached the update tour method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != tour.Tourid)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + tour.Tourid);
                Debug.WriteLine("POST parameter" + tour.Tourname);
                Debug.WriteLine("POST parameter " + tour.Description);
                Debug.WriteLine("POST parameter " + tour.Location);
                Debug.WriteLine("POST parameter " + tour.Price);
                return BadRequest();
            }

            db.Entry(tour).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourExists(id))
                {
                    Debug.WriteLine("Tour not found");
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
        /// Searches for tours that match the given search string.
        /// </summary>
        /// <param name="searchString">The search string to filter tours.</param>
        /// <returns>A list of TourDto objects that match the search criteria.</returns>
        /// <example>
        /// GET: api/TourData/SearchTours?searchString=Adventure =>
        /// <TourDto>
        /// <TourId>789</TourId>
        /// <TourName>Adventure Trek</TourName>
        /// <Description>Explore the wilderness in this thrilling adventure.</Description>
        /// <Location>Mountains</Location>
        /// <Price>199.99</Price>
        /// </TourDto>
        /// </example>
        [HttpGet]
        [Route("api/TourData/SearchTours")]
        public List<TourDto> SearchTours(string searchString)
        {
            var tours = db.Tours
                .Where(t => t.Tourname.Contains(searchString))
                .ToList();

            return tours.Select(t => new TourDto
            {
                Tourid = t.Tourid,
                Tourname = t.Tourname,
                Description = t.Description,
                Location = t.Location,
                Price = t.Price
                // Add more properties as needed
            }).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool TourExists(int id)
        {
            return db.Tours.Count(e => e.Tourid == id) > 0;
        }

    }
}
