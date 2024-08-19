using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using FitFeastExplore.Models.View_Models;
using FitFeastExplore.Models;
using System.Web.Script.Serialization;

namespace FitFeastExplore.Controllers
{
    public class BookingController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44306/api/");
        }
        // GET: Booking/List
        public ActionResult List(string searchString)
        {



            string url = "bookingdata/listbookings";

            if (!string.IsNullOrEmpty(searchString))
            {
                url += $"?searchString={searchString}";
            }

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookingDto> BookingDtos = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            return View(BookingDtos);
        }

        //GET: Booking/Show/3
        public ActionResult Show(int id)
        {
            string url = "bookingdata/findbooking/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;

            return View(selectedBooking);
        }

        // POST: Booking/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            Debug.WriteLine("the json payload is :");

            string url = "bookingdata/addbooking";


            string jsonpayload = jss.Serialize(booking);

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
        [Authorize]
        public ActionResult New()
        {

            string url = "customersdata/listcustomers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            CreateBooking CreateBookingViewModel = new CreateBooking();
            IEnumerable<CustomerDto> CustomersOptions = response.Content.ReadAsAsync<IEnumerable<CustomerDto>>().Result;

            CreateBookingViewModel.Customers = CustomersOptions;

            url = "Tourdata/listtours";
            response = client.GetAsync(url).Result;
            IEnumerable<TourDto> tours = response.Content.ReadAsAsync<IEnumerable<TourDto>>().Result;

            CreateBookingViewModel.Tours = tours;

            return View(CreateBookingViewModel);
        }
            
        // GET: Booking/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            BookingDto selectedbooking = response.Content.ReadAsAsync<BookingDto>().Result;

            return View(selectedbooking);
        }

        // POST: Booking/Update/5
        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, Booking booking)
        {
            try
            {
                Debug.WriteLine("The new booking info is:");
                Debug.WriteLine(booking.Bookingdate);
                Debug.WriteLine(booking.Status);
                Debug.WriteLine(booking.CustomerId);
                Debug.WriteLine(booking.TourId);


                //serialize into JSON
                //Send the request to the API

                string url = "bookingdata/UpdateBooking/" + id;


                string jsonpayload = jss.Serialize(booking);
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

        // GET: Booking/Delete/5
        [Authorize]
        public ActionResult Deleteconfirm(int id)
        {
            string url = "bookingdata/findbooking/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto selectedbooking = response.Content.ReadAsAsync<BookingDto>().Result;

            return View(selectedbooking);
        }

        // POST: Booking/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deletebooking/" + id;

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
