using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;



namespace Shop.Controllers
{
   // Endpoint => URL
   // https://127.0.01:5001/categories
   [Route("categories")]
   public class CategoryController : ControllerBase
   {

      [HttpGet]
      [Route("")]
      [AllowAnonymous]
      public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
      {
         try
         {
            // Realiza apenas uma leitura rapida dos dados no DB in Memory e retorna
            // ToList é sempre no final, ele realiza as operações no banco
            var categoryList = await context.Categorys.AsNoTracking().ToListAsync();

            return Ok(categoryList);
         }
         catch (Exception)
         {

            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }

      }


      [HttpGet]
      // Verifica se o valor passado pela URL é inteiro
      [Route("{id:int}")]
      [AllowAnonymous]
      public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
      {
         try
         {

            // Busca no banco a categoria que possui o ID passado pela URL e retorna 
            var category = await context.Categorys.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
               return BadRequest(new { message = "Não foi possivel encontrar a categoria" });
            return Ok(category);
         }
         catch (Exception)
         {
            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });

         }
      }


      [HttpPost]
      [Route("")]
      [Authorize(Roles = "employee")]

      public async Task<ActionResult<Category>> Post([FromBody] Category model, [FromServices] DataContext context)
      {
         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         try
         {
            // Add model in DB Memory
            context.Categorys.Add(model);

            // Persist Category in DB
            await context.SaveChangesAsync();
            return Ok(model);
         }
         catch (Exception)
         {

            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }

      [HttpPut]
      [Route("{id:int}")]
      [Authorize(Roles = "employee")]
      public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
      {
         // Verificar se o ID informado pela URL é o mesmo da categoria
         if (id != model.Id)
            return NotFound(new { message = "Categoria não encontrada" });

         // Verifica se o modelo passado é válido
         // Caso não retorna a ErrorMessage apropriada 
         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         try
         {
            // Verifica automaticamente a modificação de valores e a realiza
            context.Entry<Category>(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
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

      [HttpDelete]
      [Route("{id:int}")]
      [Authorize(Roles = "employee")]
      public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
      {
         // Obtem a categoria que possui o ID passado pela URL
         var category = await context.Categorys.FirstOrDefaultAsync(x => x.Id == id);
         if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });

         try
         {
            // Remove a categoria encontrada anteriormente
            context.Categorys.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso " });
         }
         catch (Exception)
         {
            return BadRequest(new { message = "Ocorreu um erro. Por favor, tente novamente mais tarde" });
         }
      }
   }
}