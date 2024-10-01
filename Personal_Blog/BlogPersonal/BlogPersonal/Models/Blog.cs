using System;
using System.Collections.Generic;

namespace BlogPersonal.Models;

public partial class Blog
{
    public int CodigoBlog { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int CodigoUsuario { get; set; }

    public int CodigoEstadoBlog { get; set; }

    public virtual EstadoBlog CodigoEstadoBlogNavigation { get; set; } = null!;

    public virtual Usuario CodigoUsuarioNavigation { get; set; } = null!;
}
