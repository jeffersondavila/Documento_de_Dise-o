using System;
using System.Collections.Generic;

namespace BlogPersonal.Models;

public partial class EstadoBlog
{
    public int CodigoEstadoBlog { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
