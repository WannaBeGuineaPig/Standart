using System;
using System.Collections.Generic;

namespace Shoes_Demo_Ekz.Models;

public partial class CategoryItem
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
