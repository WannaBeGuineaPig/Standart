using System;
using System.Collections.Generic;

namespace Shoes_Demo_Ekz.Models;

public partial class OrderItem
{
    public int IdOrder { get; set; }

    public string ArticulItem { get; set; } = null!;

    public int AmountItem { get; set; }

    public virtual Item ArticulItemNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
