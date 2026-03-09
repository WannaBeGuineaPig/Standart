using System;
using System.Collections.Generic;

namespace Shestorka_API.Models;

public partial class Manafacturer
{
    public int Id { get; set; }

    public string Manafacturer1 { get; set; } = null!;

    public virtual ICollection<Eat> Eats { get; set; } = new List<Eat>();
}
