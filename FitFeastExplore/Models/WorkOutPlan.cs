using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models
{
    public class WorkOutPlan
    {
        [Key]
        public int WorkOutPlanID { get; set; }
        public string ExerciseName { get; set; }
        public int Reps { get; set; }
        public int sets { get; set; }
        public string BodyPart { get; set; }
        public string Notes { get; set; }

        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }

        [ForeignKey("WorkOut")]
        public int WorkOutId { get; set; }
        public virtual WorkOut WorkOut { get; set; }
    }

    public class WorkOutPlanDto
    {
        public int WorkOutPlanID { get; set; }
        public string ExerciseName { get; set; }
        public int Reps { get; set; }
        public int sets { get; set; }
        public string BodyPart { get; set; }
        public string Notes { get; set; }
    }
}