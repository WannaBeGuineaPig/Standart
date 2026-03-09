using System;
using System.Collections.Generic;

namespace Shoes_Demo_Ekz.Models;

public partial class Manafacturer
{
    public int Id { get; set; }

    public string Manafacturer1 { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
