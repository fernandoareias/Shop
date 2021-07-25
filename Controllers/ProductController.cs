

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;


namespace Shop.Controllers
{

   // Endpoint => URL
   // https://127.0.01:5001/produtos
   [Route("products")]
   public class ShopController : ControllerBase
   {

      [HttpGet]
      [Route("")]
      public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
      {
         try
         {
            var product = await context.Products.Include(x => x.Categorys).AsNoTracking().ToListAsync();
            if (product == null)
               return NotFound(new { message = "Não ha produto registrado" });
            return Ok(product);
         }
         catch (Exception)
         {

            return NotFound(new { message = "Não foi possivel encontrar o produto" });
         }
      }

      [HttpGet]
      [Route("{id:int}")]
      public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
      {
         try
         {
            var product = await context.Products.Include(x => x.Categorys).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
               return NotFound(new { message = "Não foi possivel encontrar o produto" });

            return Ok(product);
         }
         catch (Exception)
         {

            return NotFound(new { message = "Não foi possivel encontrar o produto" });
         }
      }

      [HttpGet] //products/categories/1
      [Route("categories/{id:int}")]
      public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
      {
         try
         {
            var products = await context
            .Products
            .Include(x => x.Categorys)
            .AsNoTracking()
            .Where(x => x.CategoryId == id)
            .ToListAsync();

            if (products == null)
               return NotFound(new { message = "Não ha produtos registrados nessa categoria" });

            return Ok(products);
         }
         catch (Exception)
         {

            return BadRequest("Não foi possivel realizar a busca");
         }
      }

      [HttpPost]
      [Route("")]

      public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
      {
         if (!ModelState.IsValid)
            return BadRequest(ModelState);
         try
         {
            context.Products.Add(model);
            await context.SaveChangesAsync();

            return Ok(model);
         }
         catch (Exception)
         {

            return BadRequest(new { message = "Não foi possivel registrar o produto" });
         }
      }


   }
}