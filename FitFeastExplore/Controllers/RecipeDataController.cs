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
    public class RecipeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all recipes in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all recipes in the database.
        /// </returns>
        /// <example>
        /// GET: api/Recipedata/Listrecipes
        /// </example>

        [HttpGet]
        [Route("api/RecipeData/ListRecipes")]
        public List<RecipeDto> ListRecipes()
        {
            List<Recipe> Recipes = db.Recipes.ToList();
            List<RecipeDto> RecipeDtos = new List<RecipeDto>();

            Recipes.ForEach(r => RecipeDtos.Add(new RecipeDto()
            {
                RecipeId = r.RecipeId,
                Category = r.Category,
                RecipeName = r.RecipeName,
                Instructions = r.Instructions,
                Protein = r.Protein,
                Calories = r.Calories,
                CookingTime = r.CookingTime,
                IngredientName = r.Ingredient.IngredientName
            }));


            return RecipeDtos;
        }

        [HttpGet]
        [Route("api/RecipeData/ListRecipesByCategory/{category}")]
        public List<RecipeDto> ListRecipesByCategory(string category)
        {
            List<Recipe> Recipes = db.Recipes.Where(r => r.Category == category).ToList();
            List<RecipeDto> RecipeDtos = new List<RecipeDto>();

            Recipes.ForEach(r => RecipeDtos.Add(new RecipeDto()
            {
                RecipeId = r.RecipeId,
                Category = r.Category,
                RecipeName = r.RecipeName,
                Instructions = r.Instructions,
                Protein = r.Protein,
                Calories = r.Calories,
                CookingTime = r.CookingTime,
                IngredientName = r.Ingredient.IngredientName
            }));

            return RecipeDtos;
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
        [ResponseType(typeof(RecipeDto))]
        [HttpGet]
        [Route("api/RecipeData/FindRecipe/{id}")]
        public IHttpActionResult FindRecipe(int id)
        {
            Recipe Recipe = db.Recipes.Find(id);

            if (Recipe == null)
            {
                return NotFound();
            }

            RecipeDto RecipeDtos = new RecipeDto()
            {
                RecipeId = Recipe.RecipeId,
                Category = Recipe.Category,
                RecipeName = Recipe.RecipeName,
                Instructions = Recipe.Instructions,
                Protein = Recipe.Protein,
                Calories = Recipe.Calories,
                CookingTime = Recipe.CookingTime,
                IngredientId = Recipe.Ingredient.IngredientId,
                IngredientName = Recipe.Ingredient.IngredientName
            };

            return Ok(RecipeDtos);
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

        [ResponseType(typeof(Recipe))]
        [HttpPost]
        [Route("api/RecipeData/AddRecipe")]
        public IHttpActionResult AddRecipe(Recipe Recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Recipes.Add(Recipe);
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

        [ResponseType(typeof(Recipe))]
        [HttpPost]
        [Route("api/RecipeData/DeleteRecipe/{id}")]
        public IHttpActionResult DeleteRecipe(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipe);
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
        [Route("api/RecipeData/UpdateRecipe/{id}")]
        public IHttpActionResult UpdateRecipe(int id, Recipe recipe)
        {
            //Debug.WriteLine("I have reached the update recipe method!");
            if (!ModelState.IsValid)
            {
                //Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeId)
            {
                //Debug.WriteLine("ID mismatch");
                //Debug.WriteLine("GET parameter" + id);
                //Debug.WriteLine("POST parameter" + recipe.RecipeId);
                //Debug.WriteLine("POST parameter" + recipe.RecipeName);
                //Debug.WriteLine("POST parameter " + recipe.Category);
                //Debug.WriteLine("POST parameter " + recipe.Instructions);
                //Debug.WriteLine("POST parameter " + recipe.Protein); 
                //Debug.WriteLine("POST parameter " + recipe.Calories);
                //Debug.WriteLine("POST parameter " + recipe.CookingTime);
                //Debug.WriteLine("POST parameter " + recipe.IngredientId);
                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    //Debug.WriteLine("Recipe not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //Debug.WriteLine("None of the conditions triggered");
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
        [Route("api/RecipeData/SearchRecipes")]
        public List<RecipeDto> SearchRecipes(string searchString)
        {
            var recipes = db.Recipes
                .Where(r => r.RecipeName.Contains(searchString))
                .ToList();

            return recipes.Select(r => new RecipeDto
            {
                RecipeId = r.RecipeId,
                RecipeName = r.RecipeName,
                IngredientId = r.IngredientId,
                Category = r.Category,
                // Add more properties as needed
            }).ToList();
        }
        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(e => e.RecipeId == id) > 0;
        }
    }
}
