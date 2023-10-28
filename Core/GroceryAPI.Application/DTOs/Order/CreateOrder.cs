namespace GroceryAPI.Application.DTOs.Order
{
    public class CreateOrder
    {
        public string? BasketId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string DeliveryTime { get; set; }
        public string PaymentOption { get; set; }
    }
}
