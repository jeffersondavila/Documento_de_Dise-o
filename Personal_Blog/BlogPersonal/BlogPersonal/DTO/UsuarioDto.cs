using System.ComponentModel.DataAnnotations;

namespace BlogPersonal.DTO
{
	public class UsuarioDto
	{
		public int CodigoUsuario { get; set; }
		public string? Nombre { get; set; }
		public string? Correo { get; set; }
		public int CodigoEstadoUsuario { get; set; }
		public DateTime? FechaUltimoAcceso { get; set; }
		public List<BlogDto> Blogs { get; set; } = new List<BlogDto>();
	}

	public class UsuarioCreateDto
	{
		[Required(ErrorMessage = "El nombre es requerido.")]
		[StringLength(200, ErrorMessage = "El nombre no debe exceder los 200 caracteres.")]
		public string? Nombre { get; set; }

		[Required(ErrorMessage = "El correo es requerido.")]
		[EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
		public string? Correo { get; set; }

		[Required(ErrorMessage = "La contraseña es requerida.")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
		public string? Password { get; set; }
	}


	public class UsuarioLoginDto
	{
		[Required(ErrorMessage = "El correo es requerido.")]
		[EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
		public string? Correo { get; set; }

		[Required(ErrorMessage = "La contraseña es requerida.")]
		public string? Password { get; set; }
	}

	public class UsuarioRecuperarPasswordDto
	{
		[Required(ErrorMessage = "El correo es requerido.")]
		[EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
		public string? Correo { get; set; }
	}

	public class UsuarioRestablecerPasswordDto
	{
		[Required(ErrorMessage = "El token de recuperación es requerido.")]
		public string? TokenRecuperacion { get; set; }

		[Required(ErrorMessage = "La nueva contraseña es requerida.")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
		public string? NuevaPassword { get; set; }
	}

}
