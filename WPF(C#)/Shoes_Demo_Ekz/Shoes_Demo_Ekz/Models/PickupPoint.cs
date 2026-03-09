using System;
using System.Collections.Generic;

namespace Shoes_Demo_Ekz.Models;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
