using System;
using System.Collections.Generic;

namespace Shestorka_API.Models;

public partial class TypeEat
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Eat> Eats { get; set; } = new List<Eat>();
}
