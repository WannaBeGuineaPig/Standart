using System.Text.Json.Serialization;

namespace Shestorka_API.DTO
{
    public class EatClass
    {
        public string? Articul { get; set; } = null;

        public string Type { get; set; } = null!;

        public string UnitOfMeasurement { get; set; } = null!;

        public int Price { get; set; }

        public string Supplier { get; set; } = null!;

        public string Manafacturer { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int? DiscountPercent { get; set; }

        public int AmountInStorage { get; set; }

        public string Description { get; set; } = null!;

        public byte[]? Image { get; set; } = null!;
    }
}
