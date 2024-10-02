namespace BlogPersonal.DTO
{
	public class UsuarioDto
	{
		public int CodigoUsuario { get; set; }
		public string? Nombre { get; set; }
		public string? Correo { get; set; }
		public int CodigoEstadoUsuario { get; set; }
		public List<BlogDto> Blogs { get; set; } = new List<BlogDto>();
	}

	public class UsuarioCreateDto
	{
		public string? Nombre { get; set; }
		public string? Correo { get; set; }
		public string? Password { get; set; }
	}

	public class UsuarioLoginDto
	{
		public string? Correo { get; set; }
		public string? Password { get; set; }
	}
}
