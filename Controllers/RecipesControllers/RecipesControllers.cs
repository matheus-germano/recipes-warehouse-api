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
        return NotFound("No recipe found");
      }

      return Ok(recipe);
    }

    [HttpPost]
    [Route("createRecipe")]
    public async Task<IActionResult> CreateRecipe([FromServices] AppDbContext context, [FromBody] Recipe recipe)
    {
      var newRecipe = new Recipe
      {
        Name = recipe.Name,
        Type = recipe.Type,
        Description = recipe.Description,
        Image = recipe.Image,
        PreparationMethod = recipe.PreparationMethod,
        Likes = 0,
        CreatedBy = recipe.CreatedBy,
        IsRecipeOwner = recipe.IsRecipeOwner,
        CreatedAt = DateTime.Now,
      };

      try
      {
        await context.Recipes.AddAsync(newRecipe);
        await context.SaveChangesAsync();
        return Ok(newRecipe);
      }
      catch (Exception e)
      {
        return BadRequest("Was not possible to create this recipe");
      }
    }

    [HttpPost]
    [Route("likeRecipe")]
    public async Task<IActionResult> LikeRecipe([FromServices] AppDbContext context, [FromBody] LikedRecipe userAndRecipeId)
    {
      var recipe = await context
        .Recipes
        .AsNoTracking()
        .FirstOrDefaultAsync(recipe => recipe.Id == userAndRecipeId.RecipeId);

      if (recipe == null)
      {
        return NotFound("No recipe found");
      }

      var recipeIsLiked = await context
        .LikedRecipes
        .AsNoTracking()
        .FirstOrDefaultAsync(likedRecipe => likedRecipe.RecipeId == userAndRecipeId.RecipeId && likedRecipe.UserId == userAndRecipeId.UserId);

      if (recipeIsLiked != null)
      {
        return BadRequest("You already liked this recipe");
      }

      try
      {
        recipe.Likes++;

        context.Recipes.Update(recipe);
        await context.SaveChangesAsync();
      }
      catch (Exception e)
      {
        return BadRequest("Ocurred an error while trying to like the recipe");
      }

      var likedRecipe = new LikedRecipe
      {
        RecipeId = userAndRecipeId.RecipeId,
        UserId = userAndRecipeId.UserId,
      };

      try
      {
        await context.LikedRecipes.AddAsync(likedRecipe);
        await context.SaveChangesAsync();

        return Ok("Liked recipe registered");
      }
      catch (Exception e)
      {
        return BadRequest("Was not possible to like the recipe");
      }
    }
  }
}