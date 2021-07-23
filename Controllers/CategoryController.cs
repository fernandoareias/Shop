using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;



// Endpoint => URL
// https://127.0.01:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{

   [HttpGet]
   [Route("")]
   public async Task<ActionResult<List<Category>>> Get()
   {
      return Ok(new List<Category>());
   }


   [HttpGet]
   // Verifica se o valor passado pela URL é inteiro
   [Route("{id:int}")]
   public async Task<ActionResult<Category>> GetById(int id)
   {
      return Ok(new Category());
   }


   [HttpPost]
   [Route("")]

   public async Task<ActionResult<Category>> Post([FromBody] Category model)
   {
      if (!ModelState.IsValid)
         return BadRequest(ModelState);
      return Ok(model);
   }

   [HttpPut]
   [Route("{id:int}")]
   public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model)
   {
      // Verificar se o ID informado pela URL é o mesmo da categoria
      if (id != model.Id)
         return NotFound(new { message = "Categoria não encontrada" });

      // Verifica se o modelo passado é válido
      // Caso não retorna a ErrorMessage apropriada 
      if (!ModelState.IsValid)
         return BadRequest(ModelState);
      return Ok(model);
   }

   [HttpDelete]
   [Route("{id:int}")]
   public async Task<ActionResult<Category>> Delete(int id)
   {
      return Ok();
   }



}