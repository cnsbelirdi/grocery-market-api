namespace GroceryAPI.Application.DTOs.Payment
{
    public class UpdatePayment
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string? Information { get; set; }
        public float Amount { get; set; }
    }
}
