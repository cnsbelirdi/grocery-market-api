using GroceryAPI.Application.DTOs.Payment;

namespace GroceryAPI.Application.Abstractions.Services
{
    public interface IPaymentService
    {
        float TotalIncomeAmount { get; }
        float TotalExpenseAmount { get; }
        Task CreatePaymentAsync(CreatePayment createPayment);
        Task UpdatePaymentAsync(UpdatePayment updatePayment);
        Task RemovePaymentAsync(string id);
        Task<ListPayment> GetAllPaymentsAsync(int page, int size);
        Task<SinglePayment> GetPaymentByIdAsync(string id);
    }
}
