using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models
{
    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int Reps { get; set; }
        public int sets { get; set; }
        public string BodyPart { get; set; }
        public string YouTubeUrl { get; set; }
    }

    public class ExerciseDto
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int Reps { get; set; }
        public int sets { get; set; }
        public string BodyPart { get; set; }
        public string YouTubeUrl { get; set; }
    }
}