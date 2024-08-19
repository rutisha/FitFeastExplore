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
        /// GET: api/RecipeData/ListRecipes
        /// </example>

        [HttpGet]
        [Route("api/RecipeData/ListRecipes")]
        public List<RecipeDto> ListRecipes()
        {
            List<Recipe> Recipes = db.Recipes.Include(r => r.Ingredients).ToList();
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
                Ingredients = r.Ingredients.Select(i => new IngredientDto
                {
                    IngredientId = i.IngredientId,
                    IngredientName = i.IngredientName
                }).ToList()
            }));


            return RecipeDtos;
        }

        /// <summary>
        /// Returns all recipes in a specified category.
        /// </summary>
        /// <param name="category">The category of recipes.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all recipes in the specified category.
        /// </returns>
        /// <example>
        /// GET: api/RecipeData/ListRecipesByCategory/Healthy
        /// </example>

        [HttpGet]
        [Route("api/RecipeData/ListRecipesByCategory/{category}")]
        public List<RecipeDto> ListRecipesByCategory(string category)
        {
            List<Recipe> Recipes = db.Recipes.Include(r => r.Ingredients)
                                     .Where(r => r.Category == category).ToList();
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
                Ingredients = r.Ingredients.Select(i => new IngredientDto
                {
                    IngredientId = i.IngredientId,
                    IngredientName = i.IngredientName
                }).ToList()
            }));

            return RecipeDtos;
        }


        /// <summary>
        /// Gathers information about a specific recipe by its ID.
        /// </summary>
        /// <param name="id">Recipe ID.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: specific recipe in the database.
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// GET: api/RecipeData/FindRecipe/2
        /// </example>
        [ResponseType(typeof(RecipeDto))]
        [HttpGet]
        [Route("api/RecipeData/FindRecipe/{id}")]
        public IHttpActionResult FindRecipe(int id)
        {
            Recipe Recipe = db.Recipes.Include(r => r.Ingredients).FirstOrDefault(r => r.RecipeId == id);

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
                ImagePath = Recipe.ImagePath,
                Ingredients = Recipe.Ingredients.Select(i => new IngredientDto
                {
                    IngredientId = i.IngredientId,
                    IngredientName = i.IngredientName
                }).ToList()
            };

            return Ok(RecipeDtos);
        }

        /// <summary>
        /// Adds a new recipe to the system.
        /// </summary>
        /// <param name="recipe">JSON FORM DATA of a recipe</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Recipe ID, Recipe Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/RecipeData/AddRecipe
        /// FORM DATA: Recipe JSON Object
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
            return CreatedAtRoute("DefaultApi", new { id = Recipe.RecipeId }, Recipe);
        }

        /// <summary>
        /// Deletes a recipe from the system by its ID.
        /// </summary>
        /// <param name="id">The primary key of the recipe</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/RecipeData/DeleteRecipe/5
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
        /// Updates a particular recipe in the system with POST Data input.
        /// </summary>
        /// <param name="id">Represents the Recipe ID primary key</param>
        /// <param name="recipe">JSON FORM DATA of a recipe</param>
        /// <returns>
        /// HEADER: 204 (No Content)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/RecipeData/UpdateRecipe/5
        /// FORM DATA: Recipe JSON Object
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
        /// Searches for recipes that match the given search string.
        /// </summary>
        /// <param name="searchString">The search string to filter recipes.</param>
        /// <returns>A list of RecipeDto objects that match the search criteria.</returns>
        /// <example>
        /// GET: api/RecipeData/SearchRecipes?searchString=Chocolate =>
        /// <RecipeDto>
        /// <RecipeId>1</RecipeId>
        /// <RecipeName>Chocolate Cake</RecipeName>
        /// <IngredientId>123</IngredientId>
        /// <Category>Dessert</Category>
        /// </RecipeDto>
        /// </example>
        [HttpGet]
        [Route("api/RecipeData/SearchRecipes")]
        public List<RecipeDto> SearchRecipes(string searchString)
        {
            var recipes = db.Recipes.Include(r => r.Ingredients)
                            .Where(r => r.RecipeName.Contains(searchString))
                            .ToList();

            return recipes.Select(r => new RecipeDto
            {
                RecipeId = r.RecipeId,
                RecipeName = r.RecipeName,
                Category = r.Category,
                Ingredients = r.Ingredients.Select(i => new IngredientDto
                {
                    IngredientId = i.IngredientId,
                    IngredientName = i.IngredientName
                }).ToList()
            }).ToList();
        }
        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(e => e.RecipeId == id) > 0;
        }
    }
}
