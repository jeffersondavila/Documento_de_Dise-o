using System;
using System.Collections.Generic;

namespace BlogPersonal.Models;

public partial class Usuario
{
    public int CodigoUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int CodigoEstadoUsuario { get; set; }

    public DateTime? FechaUltimoAcceso { get; set; }

    public string? TokenRecuperacion { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual EstadoUsuario CodigoEstadoUsuarioNavigation { get; set; } = null!;
}
