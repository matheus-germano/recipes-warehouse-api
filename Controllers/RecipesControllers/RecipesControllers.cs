using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipes_warehouse_api.Data;
using recipes_warehouse_api.Models;

namespace recipes_warehouse_api.Controllers.RecipesControllers
{
  [ApiController]
  [Route("v1")]
  public class RecipesControllers : Controller
  {
    [HttpGet]
    [Route("recipes")]
    public async Task<IActionResult> GetAllRecipes([FromServices] AppDbContext context)
    {
      var recipes = await context
          .Recipes
          .AsNoTracking()
          .ToListAsync();

      return Ok(recipes);
    }

    [HttpGet]
    [Route("recipes/{id}")]
    public async Task<IActionResult> GetRecipe([FromServices] AppDbContext context, [FromRoute] int id)
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

    [HttpPost]
    [Route("likeRecipe")]
    public async Task<IActionResult> LikeRecipe([FromServices] AppDbContext context, [FromBody] LikedRecipe userAndRecipeId)
    {
      try 
      {
        var recipe = await context
          .Recipes
          .AsNoTracking()
          .FirstOrDefaultAsync(recipe => recipe.Id == userAndRecipeId.RecipeId);

        if (recipe == null)
        {
          return NotFound("No recipe found");
        }

        try {
          recipe.Likes++;

          context.Recipes.Update(recipe);
          await context.Recipes.SaveChangesAsync(recipes);
        } 
        catch (Exception e) 
        {
          return BadRequest("Ocurred an error");
        }
      } 
      catch (Exception e)
      {
        return BadRequest("Ocurred an error while trying to like recipe");
      }

      try 
      {
        var likedRecipe = new LikedRecipe 
        {
          RecipeId = userAndRecipeId.RecipeId,
          UserId = userToSignUp.UserId,
        };

        try
        {
          await context.LikedRecipes.AddAsync(user);
          await context.SaveChangesAsync();

          return Ok("User registered");
        }
        catch (Exception e)
        {
          return BadRequest("Was not possible to register user");
        }
      }
    }
  }
}