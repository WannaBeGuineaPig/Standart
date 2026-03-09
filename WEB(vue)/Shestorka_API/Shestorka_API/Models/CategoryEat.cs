using System;
using System.Collections.Generic;

namespace Shestorka_API.Models;

public partial class CategoryEat
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<Eat> Eats { get; set; } = new List<Eat>();
}
