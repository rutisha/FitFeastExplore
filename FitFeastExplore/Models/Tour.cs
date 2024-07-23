using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models
{
    public class Tour
    {
        [Key]
        public int Tourid { get; set; }

        public string Tourname { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public decimal Price { get; set; }

    }

    public class TourDto
    {
        public int Tourid { get; set; }

        public string Tourname { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public decimal Price { get; set; }

    }
}