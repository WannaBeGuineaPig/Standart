using System;
using System.Collections.Generic;

namespace Shoes_Demo_Ekz.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly DateOrdering { get; set; }
    public string DateOrderingString
    {
        get
        {
            string day = DateOrdering.Day > 9 ? DateOrdering.Day.ToString() : $"0{DateOrdering.Day}";
            string month = DateOrdering.Month > 9 ? DateOrdering.Month.ToString() : $"0{DateOrdering.Month}";
            return $"{day}.{month}.{DateOrdering.Year}";
        }
    }

    public DateOnly DateDelivery { get; set; }
    public string DateDeliveryString
    {
        get
        {
            string day = DateDelivery.Day > 9 ? DateDelivery.Day.ToString() : $"0{DateDelivery.Day}";
            string month = DateDelivery.Month > 9 ? DateDelivery.Month.ToString() : $"0{DateDelivery.Month}";
            return $"{day}.{month}.{DateDelivery.Year}";
        }
    }
    public int IdPickupPoint { get; set; }

    public int IdUser { get; set; }

    public int CodePickup { get; set; }

    public string Status { get; set; } = null!;

    public virtual PickupPoint IdPickupPointNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
