using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FitFeastExplore.Models;

namespace FitFeastExplore.Controllers
{
    public class IngredientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // Static constructor to initialize the HttpClient
        static IngredientController()
        {
            client = new HttpClient();
            // Set the base address of the API
            client.BaseAddress = new Uri("https://localhost:44306/api/");
        }

        // GET: Ingredient/List
        // This action lists all ingredients or filters by search string if provided
        public ActionResult List(string searchString)
        {
            string url = "ingredientdata/listingredients";

            if (!string.IsNullOrEmpty(searchString))
            {
                url += $"?searchString={searchString}";
            }

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<IngredientDto> IngredientDtos = response.Content.ReadAsAsync<IEnumerable<IngredientDto>>().Result;

            return View(IngredientDtos);
        }

        //GET: Ingredient/Show/2
        public ActionResult Show(int id)
        {
            string url = "ingredientdata/findingredient/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            IngredientDto selectedIngredient = response.Content.ReadAsAsync<IngredientDto>().Result;

            return View(selectedIngredient);
        }

        // POST: Ingredient/Create
        [HttpPost]
        public ActionResult Create(Ingredient ingredient)
        {
            Debug.WriteLine("the json payload is :");

            string url = "ingredientdata/addingredient";


            string jsonpayload = jss.Serialize(ingredient);

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

        // This action returns an error view
        public ActionResult Error()
        {
            return View();
        }

        // GET: Ingredient/Edit/3
        // This action retrieves the ingredient data for editing by ID
        public ActionResult Edit(int id)
        {
            string url = "ingredientdata/findingredient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IngredientDto selectedingredient = response.Content.ReadAsAsync<IngredientDto>().Result;

            return View(selectedingredient);
        }

        // POST: Ingredient/Update/3
        [HttpPost]
        public ActionResult Update(int id, Ingredient ingredient)
        {
            try
            {
                Debug.WriteLine("The new recipe info is:");
                Debug.WriteLine(ingredient.IngredientName);
                Debug.WriteLine(ingredient.Quantity);



                //serialize into JSON
                //Send the request to the API

                string url = "ingredientdata/UpdateIngredient/" + id;


                string jsonpayload = jss.Serialize(ingredient);
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

        // GET: Ingredient/Delete/3
        public ActionResult Deleteconfirm(int id)
        {
            string url = "ingredientdata/findingredient/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            IngredientDto selectedingredient = response.Content.ReadAsAsync<IngredientDto>().Result;

            return View(selectedingredient);
        }

        // POST: Ingredients/Delete/3
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleteingredient/" + id;

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