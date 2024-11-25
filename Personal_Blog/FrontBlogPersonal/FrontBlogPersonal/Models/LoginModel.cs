using System.ComponentModel.DataAnnotations;

namespace FrontBlogPersonal.Models
{
	public class LoginModel
	{
		//[Required(ErrorMessage = "El correo es obligatorio.")]
		//[EmailAddress(ErrorMessage = "El correo no tien un formato válido.")]
		public string? Correo { get; set; }

		//[Required(ErrorMessage = "El correo es obligatorio.")]
		//[EmailAddress(ErrorMessage = "El correo no tien un formato válido.")]
		public string? Password { get; set; }
	}
}
