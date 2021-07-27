using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
   [Route("v1")]
   public class HomeController : ControllerBase
   {
      [HttpGet]
      public async Task<ActionResult<User>> Get([FromServices] DataContext context)
      {
         try
         {
            var user = new User
            {
               Id = 1,
               Username = "admin",
               Password = "Q#ajwCk@m6KAng",
               Role = "manager"

            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Ok(new { message = "Default user gen" });
         }
         catch (System.Exception)
         {

            return BadRequest(new { message = "Não foi possivel encontrar o produto" });
         }
      }
   }
}