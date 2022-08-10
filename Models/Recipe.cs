using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipes_warehouse_api.Models
{
  public class Recipe
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string PreparationMethod { get; set; }
    public int Likes { get; set; }
    public IList<User> Users { get; set; }
  }
}