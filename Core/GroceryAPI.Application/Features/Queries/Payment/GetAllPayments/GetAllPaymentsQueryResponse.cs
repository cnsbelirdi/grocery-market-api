namespace GroceryAPI.Application.Features.Queries.Payment.GetAllPayments
{
    public class GetAllPaymentsQueryResponse
    {
        public int TotalPaymentCount { get; set; }
        public object Payments { get; set; }
    }
}
