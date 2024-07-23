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
    public class CustomerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all customers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all customers in the database.
        /// </returns>
        /// <example>
        /// GET: api/Customerdata/ListCustomers
        /// </example>

        [HttpGet]
        public List<CustomerDto> ListCustomers()
        {
            List<Customer> Customers = db.Customers.ToList();
            List<CustomerDto> CustomerDtos = new List<CustomerDto>();

            Customers.ForEach(c => CustomerDtos.Add(new CustomerDto()
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Email = c.Email
            }));

            return CustomerDtos;
        }
        /// <summary>
        /// Gathers information about customer information.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: specific customer in the database.
        /// </returns>
        /// <param name="id">Customer ID.</param>
        /// <example>
        /// GET: api/Customerdata/FindCustomer/2
        /// </example>

        [ResponseType(typeof(Customer))]
        [HttpGet]
        [Route("api/Customerdata/FindCustomer/{id}")]
        public IHttpActionResult FindCustomer(int id)
        {
            Customer Customer = db.Customers.Find(id);

            if (Customer == null)
            {
                return NotFound();
            }

            CustomerDto CustomerDtos = new CustomerDto()
            {
                CustomerId = Customer.CustomerId,
                CustomerName = Customer.CustomerName,
                Email = Customer.Email

            };

            return Ok(CustomerDtos);
        }

        /// <summary>
        /// Adds an new customer to the system
        /// </summary>
        /// <param name="customer">JSON FORM DATA of an customer</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Customer ID, Customer Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// // POST: api/CustomerData/AddCustomer
        /// FORM DATA: Customer JSON Object
        /// </example>

        [ResponseType(typeof(Customer))]
        [HttpPost]
        [Route("api/CustomerData/AddCustomer")]
        public IHttpActionResult AddCustomer(Customer Customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(Customer);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an customer from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the customer</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CustomerData/DeleteCustomer/5
        /// FORM DATA: (empty)

        [ResponseType(typeof(Customer))]
        [HttpPost]
        [Route("api/CustomerData/DeleteCustomer/{id}")]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates a particular customer in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the customer ID primary key</param>
        /// <param name="customer">JSON FORM DATA of an customer</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CustomerData/UpdateCustomer/5
        /// FORM DATA: Tour JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/CustomerData/UpdateCustomer/{id}")]
        public IHttpActionResult UpdateCustomer(int id, Customer customer)
        {
            Debug.WriteLine("I have reached the update tour method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + customer.CustomerId);
                Debug.WriteLine("POST parameter" + customer.CustomerName);
                Debug.WriteLine("POST parameter " + customer.Email);
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    Debug.WriteLine("Customer not found");
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
        /// Searches for customers that match the given search string.
        /// </summary>
        /// <param name="searchString">The search string to filter customers.</param>
        /// <returns>A list of CustomerDto objects that match the search criteria.</returns>
        /// <example>
        /// GET: api/CustomerData/SearchCustomers?searchString=John =>
        /// <CustomerDto>
        /// <CustomerId>123</CustomerId>
        /// <CustomerName>John Doe</CustomerName>
        /// <Email>john.doe@example.com</Email>
        /// </CustomerDto>
        /// </example>
        [HttpGet]
        [Route("api/CustomerData/SearchCustomers")]
        public List<CustomerDto> SearchCustomers(string searchString)
        {
            var customers = db.Customers
                .Where(c => c.CustomerName.Contains(searchString))
                .ToList();

            return customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Email = c.Email
                // Add more properties as needed
            }).ToList();
        }
        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }

    }
}
