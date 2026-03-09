using System;
using System.Collections.Generic;

namespace Shestorka_API.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly DateOrdering { get; set; }

    public DateOnly DateDelivery { get; set; }

    public int IdPickupPoint { get; set; }

    public int IdUser { get; set; }

    public int CodePickup { get; set; }

    public string Status { get; set; } = null!;

    public virtual PickupPoint IdPickupPointNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
