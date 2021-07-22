

namespace Shop.Models
{
   public class User
   {
      [Key]
      public int Id { get; set; }

      [Requerid(ErrorMessage = "Este campo é obrigatório")]
      [MaxLenght(20, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres")]
      [MinLenght(3, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres")]
      public string Username { get; set; }

      [Requerid(ErrorMessage = "Este campo é obrigatório")]
      [MaxLenght(20, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres")]
      [MinLenght(3, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres")]
      public string Password { get; set; }


      public string Role { get; set; }
   }
}