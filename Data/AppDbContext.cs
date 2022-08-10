using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using recipes_warehouse_api.Models;

namespace recipes_warehouse_api.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<LikedRecipe>()
       .HasKey(table => new
       {
         table.UserId,
         table.RecipeId
       });

      // builder.Entity<LikedRecipe>()
      //   .HasOne(x => x.User)
      //   .WithMany(y => y.Recipes)
      //   .HasForeignKey(y => y.RecipeId);

      // builder.Entity<LikedRecipe>()
      //   .HasOne(x => x.Recipe)
      //   .WithMany(y => y.Users)
      //   .HasForeignKey(y => y.UserId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<LikedRecipe> LikedRecipes { get; set; }
  }
}