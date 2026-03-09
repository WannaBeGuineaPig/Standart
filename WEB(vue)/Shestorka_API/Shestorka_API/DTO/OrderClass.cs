namespace Shestorka_API.DTO
{
    public class OrderClass
    {
        public int IdUser { get; set; }
        public DateOnly DateOrdering { get; set; }
        public DateOnly DateDelivery { get; set; }
        public string Status { get; set; }
        public string AddressPickUpPoint { get; set; }
        public Dictionary<string, int> ItemOrder { get; set; }
    }
}
