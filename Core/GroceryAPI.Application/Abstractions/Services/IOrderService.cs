using GroceryAPI.Application.DTOs.Order;

namespace GroceryAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        int TotalOrderCount { get; }
        int TotalCompletedOrderCount { get; }
        int TotalWaitingOrderCount { get; }
        int TotalCanceledOrderCount { get; }
        Task CreateOrderAsync(CreateOrder createOrder);
        Task<ListOrder> GetAllOrdersAsync(int page, int size);
        Task<SingleOrder> GetOrderByNumber(string number);
        Task<(bool, CompletedOrder)> CompleteOrderAsync(string id);
        Task CancelOrderAsync(string id);
        Task<ListOrder> GetOrdersByUser(string userId);
    }
}
