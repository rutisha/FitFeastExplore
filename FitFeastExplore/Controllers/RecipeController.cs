using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ActionResult ListByCategory(string category)
        {
            string url = $"recipedata/listrecipesbycategory/{category}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RecipeDto> recipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;

            ViewBag.Category = category;
            return View(recipes);
        }


        //GET: Recipe/Show/3
        public ActionResult Show(int id)
        {
            string url = "recipedata/findrecipe/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(Recipe);
        }

        //Get: Recipe/New
        public ActionResult New()
        {
            return View();
        }


        // POST: Recipe/Create
        [HttpPost]
        public ActionResult Create(Recipe recipe)
        {
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

        public ActionResult Error()
        {
            return View();
        }

        // GET: Recipe/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "recipedata/findrecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(Recipe);
        }

        // POST: Recipe/Update/5
        [HttpPost]
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
                Debug.WriteLine(recipe.IngredientId);



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

        // GET: Recipe/Delete/5
        public ActionResult Deleteconfirm(int id)
        {
            string url = "recipedata/findrecipe/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto selectedrecipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(selectedrecipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost]
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