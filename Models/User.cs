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
      [MaxLength(8, ErrorMessage = "Esse campo deve conter no minimo 8 caracteres")]
      public string Password { get; set; }

      public string Role { get; set; }
   }

}