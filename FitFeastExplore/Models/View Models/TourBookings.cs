using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models.View_Models
{
    public class TourBookings
    {
        public TourDto SelectedTour { get; set; }
        public IEnumerable<BookingDto> RelatedCustomers { get; set; }
    }
}