using System.ComponentModel.DataAnnotations;


namespace Shop.Models
{
   public class User
   {
      [Key]
      public int Id { get; set; }


      [Required(ErrorMessage = "Esse campo é obrigatório")]
      [MaxLength(30, ErrorMessage = "Esse campo deve conter no máximo 30 caracteres")]
      public string Username { get; set; }


      [Required(ErrorMessage = "Esse campo é obrigatório")]
      [MinLength(8, ErrorMessage = "Esse campo deve conter no minimo 8 caracteres")]
      public string Password { get; set; }

      [MaxLength(30, ErrorMessage = "Esse campo deve conter no máximo 30 caracteres")]
      [MinLength(3, ErrorMessage = "Esse campo deve conter no minimo 3 caracteres")]
      public string Role { get; set; }
   }

}