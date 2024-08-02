using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models
{
    public class WorkOutDetailsViewModel
    {
        public WorkOutDto WorkOut { get; set; }
        public List<ExerciseDto> Exercises { get; set; }

        public int WorkOutPlanID { get; set; }
    }
}