using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models.View_Models
{
    public class CreateBooking
    {
        public IEnumerable<TourDto> Tours { get; set; }

        public IEnumerable<CustomerDto> Customers { get; set; }
    }
}