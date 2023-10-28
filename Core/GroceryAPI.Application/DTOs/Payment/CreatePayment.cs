namespace GroceryAPI.Application.DTOs.Payment
{
    public class CreatePayment
    {
        public string Type { get; set; }
        public string? Information { get; set; }
        public float Amount { get; set; }
    }
}
