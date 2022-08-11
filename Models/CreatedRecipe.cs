using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipes_warehouse_api.Models
{
  public class CreatedRecipe
  {
    public string UserId { get; set; }
    public int RecipeId { get; set; }

    public User User { get; set; }
    public Recipe Recipe { get; set; }
  }
}