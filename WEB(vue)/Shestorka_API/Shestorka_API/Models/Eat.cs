using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shestorka_API.Models;

public partial class Eat
{
    public string Articul { get; set; } = null!;

    public int IdType { get; set; }

    public string UnitOfMeasurement { get; set; } = null!;

    public int Price { get; set; }

    public int IdSupplier { get; set; }

    public int IdManafacturer { get; set; }

    public int IdCategory { get; set; }

    public int? DiscountPercent { get; set; }

    public int AmountInStorage { get; set; }

    public string Description { get; set; } = null!;

    public byte[]? Image { get; set; }

    public virtual CategoryEat IdCategoryNavigation { get; set; } = null!;

    public virtual Manafacturer IdManafacturerNavigation { get; set; } = null!;

    public virtual Supplier IdSupplierNavigation { get; set; } = null!;

    public virtual TypeEat IdTypeNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
