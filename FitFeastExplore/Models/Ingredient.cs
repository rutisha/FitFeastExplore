using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string Quantity { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }

    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string Quantity { get; set; }
    }
}