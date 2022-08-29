using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipes_warehouse_api.Data;
using recipes_warehouse_api.Entities;
using recipes_warehouse_api.Models;
using SecureIdentity.Password;

namespace recipes_warehouse_api.Controllers.UserControllers
{
  [ApiController]
  [Route("v1")]
  public class UserControllers : Controller
  {
    [HttpPost]
    [Route("sign-in")]
    public async Task<IActionResult> SignIn([FromServices] AppDbContext context, [FromBody] UserToSignIn userToSignIn)
    {
      var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == userToSignIn.Email);

      if (user == null)
      {
        return NotFound("No user with this email or password");
      }

      if (!PasswordHasher.Verify(user.Password, userToSignIn.Password))
      {
        return NotFound("No user with this email or password");
      }

      return Ok(user);
    }

    [HttpPost]
    [Route("sign-up")]
    public async Task<IActionResult> SignUp([FromServices] AppDbContext context, [FromBody] User userToSignUp)
    {
      var userAlreadyExists = await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == userToSignUp.Email);

      if (userAlreadyExists != null)
      {
        return BadRequest(409);
      }

      var user = new User
      {
        Id = userToSignUp.Id,
        Name = userToSignUp.Name,
        Email = userToSignUp.Email,
        Password = PasswordHasher.Hash(userToSignUp.Password),
        Image = userToSignUp.Image
      };

      try
      {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return Ok("User registered");
      }
      catch (Exception e)
      {
        return BadRequest("Was not possible to register user");
      }
    }

    [HttpPut]
    [Route("editProfile")]
    public async Task<IActionResult> EditProfile([FromServices] AppDbContext context, [FromBody] UserToEdit userToEdit)
    {
      var user = await context
        .Users
        .FirstOrDefaultAsync(user => user.Id == userToEdit.Id);

      if (user == null)
      {
        return NotFound("Usuario nao encontrado");
      }

      try
      {
        user.Name = userToEdit.Name;
        user.Image = userToEdit.Image;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Ok("Dados atualizados");
      }
      catch (Exception e)
      {
        return BadRequest("Ocorreu um erro");
      }
    }
  }
}