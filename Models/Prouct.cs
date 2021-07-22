
namespace Shop.Models
{
   public class Product
   {
      [Key]
      public int Id { get; set; }


      [MaxLenght(60, ErrorMessage = "Este campo deve conter no maximo 60 caracters")]
      [MinLenght(3, ErrorMessage = "Este campo deve conter no minimo 3 caracters")]
      public string Title { get; set; }


      [MaxLenght(1024, ErrorMessage = "Este campo deve conter no maximo 1024 caracters")]
      public string Description { get; set; }


      [Requerid(ErrorMessage = "Este campo é obrigatorio")]
      [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
      public decimal Price { get; set; }


      [Requerid(ErrorMessage = "Este campo é obrigatorio")]
      public Category Categorys { get; set; }


   }
}