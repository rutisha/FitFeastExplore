using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models
{
    public class WorkOut
    {
        [Key]
        public int WorkOutId { get; set; }
        public string WorkOutName { get; set; }
    }

    public class WorkOutDto
    {
        public int WorkOutId { get; set; }
        public string Name { get; set; }
    }
}