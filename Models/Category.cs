using System.ComponentModel.DataAnnotation;

namespace Shop.Models
{
   public class Category
   {
      [Key]
      public int Id { get; set; }


      [Requerid(ErrorMessage = "Este campo é obrigatório")]
      [MaxLength(ErrorMessage = "Este campo deve conter entre 3 a 60 caracteres")]
      [MinLength(ErrorMessage = "Este campo deve conter entre 3 a 60 caracteres")]
      [DataType("nvarchar")]
      public string Title { get; set; }
   }
}