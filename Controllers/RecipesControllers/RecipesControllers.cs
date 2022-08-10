using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipes_warehouse_api.Data;

namespace recipes_warehouse_api.Controllers.RecipesControllers
{
  [ApiController]
  [Route("v1")]
  public class RecipesControllers : Controller
  {
    [HttpGet]
    [Route("recipes")]
    public async Task<IActionResult> getAllRecipes([FromServices] AppDbContext context)
    {
      var recipes = await context
          .Recipes
          .AsNoTracking()
          .ToListAsync();

      return Ok(recipes);
    }

    [HttpGet]
    [Route("recipes/{id}")]
    public async Task<IActionResult> getRecipe([FromServices] AppDbContext context, [FromRoute] int id)
    {
      var recipe = await context
          .Recipes
          .AsNoTracking()
          .FirstOrDefaultAsync(recipe => recipe.Id == id);

      if (recipe == null)
      {
        return NotFound("Nenhuma receita encontrada");
      }

      return Ok(recipe);
    }
  }
}