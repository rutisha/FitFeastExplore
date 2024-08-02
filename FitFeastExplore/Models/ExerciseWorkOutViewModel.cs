using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitFeastExplore.Models
{
    public class ExerciseWorkOutViewModel
    {
        public ExerciseDto Exercise { get; set; }
        public int WorkOutId { get; set; }
        public List<SelectListItem> WorkOuts { get; set; }
    }
}