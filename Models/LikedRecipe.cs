using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipes_warehouse_api.Models
{
    public class LikedRecipe
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public int RecipeId { get; set; }
    }
}