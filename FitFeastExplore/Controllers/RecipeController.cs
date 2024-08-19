using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FitFeastExplore.Migrations;
using FitFeastExplore.Models;
using FitFeastExplore.Models.View_Models;
using Microsoft.AspNet.Identity;

namespace FitFeastExplore.Controllers
{
    public class RecipeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RecipeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44306/api/");
        }

        /// <summary>
        /// Displays a list of all recipes, with optional search functionality.
        /// </summary>
        /// <param name="searchString">The search string to filter recipes.</param>
        /// <returns>A view displaying a list of RecipeDto objects.</returns>
        // GET: Recipe/List
        public ActionResult List(string searchString)
        {
            string url = "recipedata/listrecipes";

            if (!string.IsNullOrEmpty(searchString))
            {
                url += $"?searchString={searchString}";
            }

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RecipeDto> RecipeDtos = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;
            //Debug.WriteLine("Number of recipes in the list: " + RecipeDtos.Count());

            return View(RecipeDtos);
        }


        /// <summary>
        /// Displays a list of recipes filtered by category.
        /// </summary>
        /// <param name="category">The category to filter recipes by.</param>
        /// <returns>A view displaying a list of RecipeDto objects filtered by category.</returns>
        public ActionResult ListByCategory(string category)
        {
            string url = $"recipedata/listrecipesbycategory/{category}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RecipeDto> recipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;

            ViewBag.Category = category;
            return View(recipes);
        }

        /// <summary>
        /// Displays details for a specific recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe to display.</param>
        /// <returns>A view displaying the details of the specified RecipeDto object.</returns>
        // GET: Recipe/Show/3
        public ActionResult Show(int id)
        {
            string url = "recipedata/findrecipe/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            //Debug.WriteLine("Ingredients count: " + Recipe.Ingredients.Count);

            return View(Recipe);
        }

        /// <summary>
        /// Displays the form to create a new recipe.
        /// </summary>
        /// <returns>A view displaying the form to create a new recipe.</returns>
        // GET: Recipe/New
        [Authorize]
        public ActionResult New()
        {
            string url = "ingredientdata/listingredients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<IngredientDto> ingredients = response.Content.ReadAsAsync<IEnumerable<IngredientDto>>().Result;

            // Pass ingredients to the view
            ViewBag.Ingredients = new SelectList(ingredients, "IngredientId", "IngredientName");

            return View();
        }

        /// <summary>
        /// Handles the creation of a new recipe.
        /// </summary>
        /// <param name="recipe">The Recipe object to create.</param>
        /// <returns>A redirect to the List action if successful, otherwise an Error view.</returns>
        // POST: Recipe/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Recipe recipe, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/Recipes"), fileName);
                ImageFile.SaveAs(path);
                recipe.ImagePath = "/Images/Recipes/" + fileName;
            }
            Debug.WriteLine("the json payload is :");

            string url = "recipedata/addrecipe";


            string jsonpayload = jss.Serialize(recipe);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays an error view.
        /// </summary>
        /// <returns>A view displaying an error message.</returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Displays the form to edit an existing recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe to edit.</param>
        /// <returns>A view displaying the form to edit the specified RecipeDto object.</returns>
        // GET: Recipe/Edit/3
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "recipedata/findrecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(Recipe);
        }

        /// <summary>
        /// Handles the update of an existing recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe to update.</param>
        /// <param name="recipe">The updated Recipe object.</param>
        /// <returns>A redirect to the Show action if successful, otherwise the Edit view.</returns>
        // POST: Recipe/Update/3
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Recipe recipe)
        {
            try
            {
                Debug.WriteLine("The new recipe info is:");
                Debug.WriteLine(recipe.RecipeName);
                Debug.WriteLine(recipe.Instructions);
                Debug.WriteLine(recipe.Category);
                Debug.WriteLine(recipe.Protein);
                Debug.WriteLine(recipe.Calories);
                Debug.WriteLine(recipe.CookingTime);
                //Debug.WriteLine(recipe.IngredientId);



                //serialize into JSON
                //Send the request to the API

                string url = "recipedata/UpdateRecipe/" + id;


                string jsonpayload = jss.Serialize(recipe);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;


                return RedirectToAction("Show/" + id);
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Displays a confirmation view for deleting a recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        /// <returns>A view displaying the details of the RecipeDto object to be deleted.</returns>
        // GET: Recipe/Delete/2
        [Authorize]
        public ActionResult Deleteconfirm(int id)
        {
            string url = "recipedata/findrecipe/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto selectedrecipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(selectedrecipe);
        }

        /// <summary>
        /// Handles the deletion of a recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        /// <returns>A redirect to the List action if successful, otherwise an Error view.</returns>
        // POST: Recipe/Delete/2
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "deleterecipe/" + id;

            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}