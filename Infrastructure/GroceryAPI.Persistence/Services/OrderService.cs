using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.DTOs.Order;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IPaymentService _paymentService;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, IBasketReadRepository basketReadRepository, IPaymentService paymentService)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _basketReadRepository = basketReadRepository;
            _paymentService = paymentService;
        }

        public int TotalOrderCount => _orderReadRepository.Table.Count();
        public int TotalCompletedOrderCount => _orderReadRepository.Table.Where(o => o.Status == "Completed").Count(); 
        public int TotalWaitingOrderCount => _orderReadRepository.Table.Where(o => o.Status == "Waiting").Count();
        public int TotalCanceledOrderCount => _orderReadRepository.Table.Where(o => o.Status == "Canceled").Count();
        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            var basketId = Guid.Parse(createOrder.BasketId);
            var basket = await _basketReadRepository.Table
                .Include(b => b.BasketItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(b => b.Id == basketId);

            if (basket == null) return;

            var totalPrice = basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity);
            // Kargo ücreti
            if(totalPrice < 8)
            {
                totalPrice += 5;
            }

            //Stok eksiltme
            foreach (var basketItem in basket.BasketItems)
            {
                basketItem.Product.Stock -= basketItem.Quantity;
                if (basketItem.Product.Stock == 0)
                    basketItem.Product.Active = false;
            }

            await _orderWriteRepository.AddAsync(new()
            {
                Id = basketId,
                Address = createOrder.Address,
                Description = createOrder.Description,
                OrderNumber = Math.Round(new Random().NextDouble() * 10000000000).ToString(),
                TotalPrice = totalPrice,
                DeliveryTime = createOrder.DeliveryTime,
                PaymentOption = Convert.ToInt32(createOrder.PaymentOption),
            } );
            await _orderWriteRepository.SaveAsync();
        }

        public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
        {
            var query = _orderReadRepository.Table.Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .Include(o => o.Basket)
                    .ThenInclude(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                    .OrderByDescending(o => o.CreatedDate);

            IQueryable<Order> data = null;
            if (page != -1 && size != -1)
                data = query.Skip(page * size).Take(size);
            else
                data = query;

            return new()
            {
                TotalOrderCount = await query.CountAsync(),
                Orders = await data.Select(o => new
                {
                    o.Id,
                    CreatedDate = o.CreatedDate.ToString("dd.MM.yyyy"),
                    o.OrderNumber,
                    o.TotalPrice,
                    Username = o.Basket.User.UserName,
                    o.Status,
                    o.DeliveryTime
                }).ToListAsync()
            };
        }

        public async Task<SingleOrder> GetOrderByNumber(string number)
        {
            var data = await _orderReadRepository.Table
                .Include(o => o.Basket)
                    .ThenInclude(b => b.BasketItems)
                        .ThenInclude(bi => bi.Product)
                .Include(u => u.Basket.User)
                                .FirstOrDefaultAsync(o => o.OrderNumber == number);
            return new()
            {
                Id = data.Id.ToString(),
                BasketItems = data.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity,
                }),
                Address = data.Address,
                DeliveryTime = data.DeliveryTime,
                PaymentOption = data.PaymentOption,
                TotalPrice = data.TotalPrice,
                CreatedDate = data.CreatedDate,
                OrderNumber = data.OrderNumber,
                Description = data.Description,
                Status = data.Status,
                Fullname = data.Basket.User.FullName,
                PhoneNumber = data.Basket.User.PhoneNumber
            };
        }

        public async Task<ListOrder> GetOrdersByUser (string userId)
        {
            var query = _basketReadRepository.Table
                        .Include(b => b.Order)
                        .Where(b => b.UserId == userId && b.Order != null)
                        .OrderByDescending(o => o.Order.CreatedDate);
            return new()
            {
                TotalOrderCount = await query.CountAsync(),
                Orders = await query.Select(b => new
                {
                    OrderId = b.Order.Id,
                    CreatedDate = b.Order.CreatedDate.ToString("dd.MM.yyyy"),
                    b.Order.OrderNumber,
                    b.Order.TotalPrice,
                    b.Order.Status
                }).ToListAsync()
            };
        }

        public async Task<(bool, CompletedOrder)> CompleteOrderAsync(string id)
        {
            Order? order = await _orderReadRepository.Table.Include(b => b.Basket)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(b => b.Id == Guid.Parse(id));
            if (order != null)
            {
                order.Status = "Completed";
                order.UpdatedDate = DateTime.Now;
                await _paymentService.CreatePaymentAsync(new() {
                    Type = "IncomePayment",
                    Information = "Order: " + order.OrderNumber,
                    Amount = order.TotalPrice,
                });
                return (await _orderWriteRepository.SaveAsync() > 0, new() { 
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.CreatedDate,
                    FullName = order.Basket.User.FullName,
                    Email = order.Basket.User.Email,
                });
            }
            return (false, new());
        }

        public async Task CancelOrderAsync(string id)
        {
            Order? order = await _orderReadRepository.Table
                .Include(o => o.Basket)
                    .ThenInclude(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

            if (order != null)
            {
                order.Status = "Canceled";
                order.UpdatedDate = DateTime.Now;
                foreach (var basketItem in order.Basket.BasketItems)
                {
                    basketItem.Product.Stock += basketItem.Quantity;
                    if (!basketItem.Product.Active)
                        basketItem.Product.Active = true;
                }
                await _orderWriteRepository.SaveAsync();
            }
        }


    }
}
