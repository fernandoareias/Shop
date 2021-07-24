using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;



// Endpoint => URL
// https://127.0.01:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{

   [HttpGet]
   [Route("")]
   public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
   {
      // Realiza apenas uma leitura rapida dos dados no DB in Memory e retorna
      // ToList é sempre no final, ele realiza as operações no banco
      var categoryList = await context.Categorys.AsNoTracking().ToListAsync();

      return Ok(categoryList);

   }


   [HttpGet]
   // Verifica se o valor passado pela URL é inteiro
   [Route("{id:int}")]
   public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
   {
      // Busca no banco a categoria que possui o ID passado pela URL e retorna 
      var category = await context.Categorys.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      
      return Ok(category);
   }


   [HttpPost]
   [Route("")]

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
      catch
      {

         return BadRequest(new { message = "Não foi possivel criar a categoria" });
      }
   }

   [HttpPut]
   [Route("{id:int}")]
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

         return BadRequest(new { message = "Não foi possivel atualizar a categoria" });
      }
   }

   [HttpDelete]
   [Route("{id:int}")]
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

         return BadRequest(new { message = "Não foi possivel remover a categoria" });
      }

   }



}