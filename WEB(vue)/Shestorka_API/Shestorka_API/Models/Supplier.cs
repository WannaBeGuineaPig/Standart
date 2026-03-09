using System;
using System.Collections.Generic;

namespace Shestorka_API.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string Supplier1 { get; set; } = null!;

    public virtual ICollection<Eat> Eats { get; set; } = new List<Eat>();
}
