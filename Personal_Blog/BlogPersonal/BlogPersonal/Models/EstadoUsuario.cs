using System;
using System.Collections.Generic;

namespace BlogPersonal.Models;

public partial class EstadoUsuario
{
    public int CodigoEstadoUsuario { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
