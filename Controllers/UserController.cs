using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.Services;
using System.Collections.Generic;

namespace Shop.Controllers
{
   [Route("v1/users")]
   public class UserController : Controller
   {

      // Retorna a lista de usuarios registrados
      [HttpGet]
      [Route("")] //https://127.0.0.1:5001/v1/users/
      [Authorize(Roles = "manager")]
      public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
      {
         try
         {
            var users = await context.Users.AsNoTracking().ToListAsync();
            if (users == null)
               return NotFound(new { message = "Não há usuario registrado" });

            return Ok(users);
         }
         catch (Exception)
         {

            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }


      // Caso exista, retorna o usuario que possui o ID passado via URL
      [HttpGet]
      [Route("{id:int}")] // https://127.0.0.1:5001/v1/users/1
      [Authorize(Roles = "manager")]

      public async Task<ActionResult<User>> GetById([FromServices] DataContext context, int id)
      {
         try
         {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
               return NotFound(new { message = "Usuario não encontrado" });
            return Ok(user);
         }
         catch (Exception)
         {
            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }

      // Caso exista, retorna a lista de usuario com o cargo passado via url
      [HttpGet]
      [Route("{role}")]// https://127.0.0.1:5001/v1/users/employee
      [Authorize(Roles = "manager")]
      public async Task<ActionResult<List<User>>> GetByRole([FromServices] DataContext context, String role)
      {
         try
         {
            var users = await context.Users.AsNoTracking().Where(x => x.Role == role).ToListAsync();
            if (users == null)
               return NotFound(new { message = "Não há funcionario nesse cargo" });

            return Ok(users);

         }
         catch (Exception)
         {

            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }

      }
      // Caso o modelo passado seja valido, registra um novo usuario no BD
      [HttpPost]
      [Route("")]// https://127.0.0.1:5001/v1/users/
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

            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }

      // Função responsavel pelo login do usuario
      [HttpPost]
      [Route("login")] // https://127.0.0.1:5001/v1/users/login/
      [AllowAnonymous]
      public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
      {
         try
         {
            var user = await context.Users.AsNoTracking().Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefaultAsync();
            if (user == null)
               return NotFound(new { message = "Usuario ou senha invaládios" });

            // Caso o usuario passado exista, ira gerar o Token do usuario
            var token = TokenService.GenerateToken(user);
            return new
            {
               user = user,
               token = token
            };
         }
         catch (Exception)
         {
            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }

      // Caso o usuario com ID passado pela url exista, ira atualizar os dados passados pelo body
      [HttpPut]
      [Route("{id:int}")] // https://127.0.0.1:5001/v1/users/1
      [Authorize(Roles = "manager")]

      public async Task<ActionResult<User>> Update(int id, [FromServices] DataContext context, [FromBody] User model)
      {
         if (!ModelState.IsValid)
            return BadRequest(ModelState);
         if (id != model.Id)
            return NotFound(new { message = "Usuario não encontrado" });

         try
         {
            var user = context.Entry<User>(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(user);
         }
         catch (DbUpdateConcurrencyException)
         {

            return BadRequest(new { message = "Essa categoria ja foi atualizada" });
         }
         catch (Exception)
         {
            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }


      // Caso o usuario cujo o ID passado pelo URL exista, sera removido
      [HttpDelete]
      [Route("{id:int}")] // https://127.0.0.1:5001/v1/users/1
      [Authorize(Roles = "manager")]

      public async Task<ActionResult<User>> Delete(int id, [FromServices] DataContext context)
      {
         var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
         if (user == null)
            return NotFound(new { message = "Não foi possivel encontrar o usuario" });

         try
         {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return Ok(new { message = "Usuario deletado com sucesso" });

         }
         catch (Exception)
         {

            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }

   }
}