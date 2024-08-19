using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FitFeastExplore.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Category { get; set; }
        public string RecipeName { get; set; }
        public string Instructions { get; set; }
        public string Protein { get; set; }
        public string Calories { get; set; }
        public string CookingTime { get; set; }

        //[ForeignKey("Ingredient")]
        //public int IngredientId { get; set; }
        //public virtual Ingredient Ingredient { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }


        // New properties for image
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }

    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string Category { get; set; }
        public string RecipeName { get; set; }
        public string Instructions { get; set; }
        public string Protein { get; set; }
        public string Calories { get; set; }
        public string CookingTime { get; set; }
        public List<IngredientDto> Ingredients { get; set; }

        //public int IngredientId { get; set; }
        //public string IngredientName { get; set; }
        //public string Quantity { get; set; }
        public string ImagePath { get; set; }

    }
}