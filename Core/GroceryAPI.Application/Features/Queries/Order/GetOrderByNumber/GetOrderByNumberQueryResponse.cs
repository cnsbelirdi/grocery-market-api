namespace GroceryAPI.Application.Features.Queries.Order.GetOrderByNumber
{
    public class GetOrderByNumberQueryResponse
    {
        public string OrderNumber { get; set; }
        public string Id { get; set; }
        public string Address { get; set; }
        public object BasketItems { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public float TotalPrice { get; set; }
        public int PaymentOption { get; set; }
        public string Status { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
