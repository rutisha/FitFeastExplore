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
        /// Returns all ingredients in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all ingredients in the database.
        /// </returns>
        /// <example>
        /// GET: api/IngredientData/ListIngredients
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
        /// Gathers information about a particular ingredient by its ID.
        /// </summary>
        /// <param name="id">Ingredient ID.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: specific ingredient in the database.
        /// </returns>
        /// <example>
        /// GET: api/IngredientData/FindIngredient/2
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
        /// Adds a new ingredient to the system.
        /// </summary>
        /// <param name="Ingredient">JSON FORM DATA of an ingredient</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Ingredient ID, Ingredient Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/IngredientData/AddIngredient
        /// FORM DATA: Ingredient JSON Object
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
        /// Deletes an ingredient from the system by its ID.
        /// </summary>
        /// <param name="id">The primary key of the ingredient</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/IngredientData/DeleteIngredient/5
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
        /// Updates a particular ingredient in the system with POST Data input.
        /// </summary>
        /// <param name="id">Represents the Ingredient ID primary key</param>
        /// <param name="ingredient">JSON FORM DATA of an ingredient</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/IngredientData/UpdateIngredient/3
        /// FORM DATA: Ingredient JSON Object
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
        /// Searches for ingredients that match the given search string.
        /// </summary>
        /// <param name="searchString">The search string to filter ingredients.</param>
        /// <returns>A list of IngredientDto objects that match the search criteria.</returns>
        /// <example>
        /// GET: api/IngredientData/SearchIngredients?searchString=Salt
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

        /// <summary>
        /// Checks if an ingredient exists in the database.
        /// </summary>
        /// <param name="id">The ID of the ingredient.</param>
        /// <returns>True if the ingredient exists, false otherwise.</returns>
        private bool IngredientExists(int id)
        {
            return db.Ingredients.Count(e => e.IngredientId == id) > 0;
        }
    }
}
