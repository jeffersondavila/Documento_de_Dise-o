namespace BlogPersonal.DTO
{
	public class BlogDto
	{
		public int CodigoBlog { get; set; }
		public string? Titulo { get; set; }
		public string? Contenido { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
