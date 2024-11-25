using System.ComponentModel.DataAnnotations;

namespace FrontBlogPersonal.Models
{
    public class BlogModel
    {
        public int CodigoBlog { get; set; }

        [Required(ErrorMessage = "El título es requerido.")]
        [StringLength(200, ErrorMessage = "El título no debe exceder los 200 caracteres.")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "El contenido es requerido.")]
        public string? Contenido { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int CodigoUsuario { get; set; }

        [Required(ErrorMessage = "El estado del blog es requerido.")]
        public int CodigoEstadoBlog { get; set; }
    }
}
