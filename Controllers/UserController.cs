
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
   [Route("users")]
   public class UserController : Controller
   {
      [HttpPost]
      [Route("")]
      [AllowAnonymous]
      public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
      {
         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         try
         {
            context.Users.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
         }
         catch (Exception)
         {

            return BadRequest(new { message = "Não foi possivel registrar o usuario" });
         }
      }

      [HttpPost]
      [Route("login")]
      public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
      {
         try
         {
            var user = await context.Users.AsNoTracking().Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefaultAsync();
            if (user == null)
               return NotFound(new { message = "Usuario ou senha invaládios" });
            var token = TokenService.GenerateToken(user);
            return new
            {
               user = user,
               token = token
            };
         }
         catch (Exception e)
         {
            return BadRequest($"Erro:{e.Message}");
         }




      }


   }
}