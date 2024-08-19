using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using FitFeastExplore.Models;
using FitFeastExplore.Models.View_Models;


namespace FitFeastExplore.Controllers
{
    public class TourController : Controller
    {
        public static readonly HttpClient client;
        public JavaScriptSerializer jss = new JavaScriptSerializer();

        static TourController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44306/api/");
        }
        // GET: Tour/List
        /* public ActionResult List(string searchString)
        {

            string url = "tourdata/listtours";

            if (!string.IsNullOrEmpty(searchString))
            {
                url += $"?searchString={searchString}";
            }

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TourDto> TourDtos = response.Content.ReadAsAsync<IEnumerable<TourDto>>().Result;

            return View(TourDtos);
        } */
        public ActionResult List(string searchString)
        {
            string url = "tourData/listtours";

            // Building the query parameters based on user inputs
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(searchString))
                queryParams.Add($"searchString={Uri.EscapeDataString(searchString)}");

            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<TourDto> TourDtos = response.Content.ReadAsAsync<IEnumerable<TourDto>>().Result;
                return View(TourDtos);
            }
            else
            {
                // Handle error (e.g., display an error message)
                ModelState.AddModelError("", "Unable to load tours. Please try again later.");
                return View(new List<TourDto>());
            }
        }

        //GET: Tour/Show/3
        public ActionResult Show(int id)
        {
            TourBookings ViewModel = new TourBookings();


            string url = "tourdata/findtour/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            TourDto SelectedTour = response.Content.ReadAsAsync<TourDto>().Result;

            ViewModel.SelectedTour = SelectedTour;

            url = "bookingdata/listbookingsfortour/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BookingDto> RelatedCustomers = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            ViewModel.RelatedCustomers = RelatedCustomers;

            return View(ViewModel);
        }

        // POST: Tour/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Tour Tour)
        {
            Debug.WriteLine("the json payload is :");

            string url = "tourdata/addtour";


            string jsonpayload = jss.Serialize(Tour);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);


            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(response);

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

        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // GET: Tour/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "tourdata/findtour/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            TourDto selectedtour = response.Content.ReadAsAsync<TourDto>().Result;

            return View(selectedtour);
        }

        // POST: Tour/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Tour tour)
        {
            try
            {
                Debug.WriteLine("The new tour info is:");
                Debug.WriteLine(id);
                Debug.WriteLine(tour.Tourname);
                Debug.WriteLine(tour.Description);
                Debug.WriteLine(tour.Location);
                Debug.WriteLine(tour.Price);

                //serialize into JSON
                //Send the request to the API

                string url = "tourdata/updatetour/" + id;


                string jsonpayload = jss.Serialize(tour);
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

        // GET: Tour/Delete/5
        [Authorize]
        public ActionResult Deleteconfirm(int id)
        {
            string url = "tourdata/findtour/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            TourDto selectedtour = response.Content.ReadAsAsync<TourDto>().Result;

            return View(selectedtour);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "tourdata/deletetour/" + id;

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
