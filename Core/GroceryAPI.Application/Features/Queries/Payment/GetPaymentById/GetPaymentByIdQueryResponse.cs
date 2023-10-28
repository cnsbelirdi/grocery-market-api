namespace GroceryAPI.Application.Features.Queries.Payment.GetPaymentById
{
    public class GetPaymentByIdQueryResponse
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string? Information { get; set; }
        public float Amount { get; set; }
        public string CreatedDate { get; set; }
    }
}
