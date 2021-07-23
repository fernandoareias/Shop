using Microsoft.AspNetCore.Mvc;
using Shop.Models;


// Endpoint => URL
// https://127.0.01:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{


   [HttpGet]
   // Verifica se o valor passado pela URL é inteiro
   [Route("{id:int}")]
   public string GetById(int id)
   {
      return $"GET {id}";
   }


   [HttpPost]
   [Route("")]

   public Category Post([FromBody] Category model)
   {
      return model;
   }

   [HttpPut]
   [Route("")]
   public string Put()
   {
      return "PUT";
   }

   [HttpDelete]
   [Route("")]
   public string Delete()
   {
      return "DELETE";
   }



}