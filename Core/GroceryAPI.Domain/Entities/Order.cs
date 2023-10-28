using GroceryAPI.Domain.Entities.Common;

namespace GroceryAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }
        public string Status { get; set; } = "Waiting";
        public string Address { get; set; }
        public string DeliveryTime { get; set; }
        public string OrderNumber { get; set; }
        public Basket Basket { get; set; }
        public float TotalPrice { get; set; }
        public int PaymentOption { get; set; }
    }
}
