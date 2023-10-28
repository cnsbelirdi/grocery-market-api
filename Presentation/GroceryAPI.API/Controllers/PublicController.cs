using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Constants;
using GroceryAPI.Application.CustomAttributes;
using GroceryAPI.Application.DTOs;
using GroceryAPI.Application.Enums;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        readonly IProductService _productService;
        readonly IOrderService _orderService;
        readonly IUserService _userService;
        readonly IPaymentService _paymentService;
        readonly IMailService _mailService;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IPaymentReadRepository _paymentReadRepository;
        readonly IProductReadRepository _productReadRepository;

        public PublicController(IProductService productService, IOrderService orderService, IUserService userService, IBasketItemReadRepository basketItemReadRepository, IOrderReadRepository orderReadRepository, IPaymentService paymentService, IPaymentReadRepository paymentReadRepository, IProductReadRepository productReadRepository, IMailService mailService)
        {
            _productService = productService;
            _orderService = orderService;
            _userService = userService;
            _basketItemReadRepository = basketItemReadRepository;
            _orderReadRepository = orderReadRepository;
            _paymentService = paymentService;
            _paymentReadRepository = paymentReadRepository;
            _productReadRepository = productReadRepository;
            _mailService = mailService;
        }

        [HttpGet("[action]/{table}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Public, ActionType = ActionType.Reading, Definition = "Get Table Counts")]
        public async Task<IActionResult> GetCounts([FromRoute] string table)
        {
            var tableCounts = new List<float>();

            switch (table)
            {
                case "all":
                    tableCounts.Add(_productService.TotalProductCount);
                    tableCounts.Add(_orderService.TotalOrderCount);
                    tableCounts.Add(_userService.TotalUsersCount);
                    tableCounts.Add(_paymentService.TotalIncomeAmount);
                    tableCounts.Add(_paymentService.TotalIncomeAmount - _paymentService.TotalExpenseAmount);
                    break;
                case "payments":
                    tableCounts.Add(_paymentService.TotalIncomeAmount);
                    tableCounts.Add(_paymentService.TotalExpenseAmount);
                    tableCounts.Add(_paymentService.TotalIncomeAmount - _paymentService.TotalExpenseAmount);
                    break;
                case "orders":
                    tableCounts.Add(_orderService.TotalOrderCount);
                    tableCounts.Add(_orderService.TotalCompletedOrderCount);
                    tableCounts.Add(_orderService.TotalWaitingOrderCount);
                    tableCounts.Add(_orderService.TotalCanceledOrderCount);
                    break;
                case "products":
                    tableCounts.Add(_productService.TotalProductCount);
                    tableCounts.Add(_productService.TotalStockCount);
                    tableCounts.Add(_productService.TotalActiveProductCount);
                    break;
            }
            return Ok(tableCounts);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Public, ActionType = ActionType.Reading, Definition = "Get Total Sales By Category")]
        public async Task<IActionResult> GetTotalSalesByCategory()
        {
            var salesByCategory = await _basketItemReadRepository.Table
                                        .Include(bi => bi.Product.Category)
                                        .GroupBy(bi => bi.Product.Category)
                                        .Select(g => new { CategoryName = g.Key.Name, TotalSales = g.Sum(bi => bi.Quantity) })
                                        .ToDictionaryAsync(x => x.CategoryName, x => x.TotalSales);

            return Ok(salesByCategory);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Public, ActionType = ActionType.Reading, Definition = "Get Total Orders Last 12 Days")]
        public async Task<IActionResult> GetTotalOrdersLast12Days()
        {
            Dictionary<string, int> dailyOrders = new Dictionary<string, int>();
            DateTime twelveDaysAgo = DateTime.UtcNow.AddDays(-12);

            var orders = _orderReadRepository.Table.Where(o => o.CreatedDate >= twelveDaysAgo).ToList();

            for (int i = 0; i < 12; i++)
            {
                DateTime currentDay = DateTime.UtcNow.AddDays(-i).Date;
                int count = orders.Count(o => o.CreatedDate.Date == currentDay);
                dailyOrders.Add(currentDay.ToString("dd MMM"), count);
            }

            return await Task.FromResult<IActionResult>(Ok(dailyOrders));
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Public, ActionType = ActionType.Reading, Definition = "Get Income Expense By Month")]
        public async Task<ActionResult<Dictionary<string, Dictionary<string, float>>>> GetIncomeExpenseByMonth()
        {
            var currentDate = DateTime.UtcNow;
            var sixMonthsAgo = currentDate.AddMonths(-5);

            var incomeByMonth = new Dictionary<string, float>();
            var expenseByMonth = new Dictionary<string, float>();

            for (var date = sixMonthsAgo; date <= currentDate; date = date.AddMonths(1))
            {
                var month = date.ToString("MMMM");
                var startOfMonth = new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                var endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);

                var income = await _paymentReadRepository
                    .GetWhere<IncomePayment>(p => p.CreatedDate >= startOfMonth && p.CreatedDate <= endOfMonth)
                    .SumAsync(p => p.Amount);

                var expense = await _paymentReadRepository
                    .GetWhere<ExpensePayment>(p => p.CreatedDate >= startOfMonth && p.CreatedDate <= endOfMonth)
                    .SumAsync(p => p.Amount);

                incomeByMonth.Add(month, income);
                expenseByMonth.Add(month, expense);
            }

            var result = new Dictionary<string, Dictionary<string, float>>
            {
                { "Income", incomeByMonth },
                { "Expense", expenseByMonth }
            };

            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Public, ActionType = ActionType.Reading, Definition = "Get All Waiting Orders")]
        public async Task<IActionResult> GetWaitingOrders()
        {
            IQueryable<Order> data = _orderReadRepository.Table
                    .Where(o => o.Status == "Waiting")
                    .Include(o => o.Basket)
                        .ThenInclude(b => b.User)
                    .Include(o => o.Basket)
                        .ThenInclude(b => b.BasketItems)
                        .ThenInclude(bi => bi.Product)
                    .OrderByDescending(o => o.CreatedDate);

            var orders = await data.Select(o => new
            {
                o.Id,
                CreatedDate = o.CreatedDate.ToString("dd.MM.yyyy"),
                o.OrderNumber,
                o.TotalPrice,
                Username = o.Basket.User.UserName,
                o.Status,
                o.DeliveryTime
            }).ToListAsync();

            return Ok(orders);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Public, ActionType = ActionType.Reading, Definition = "Get No Stock Products")]
        public async Task<IActionResult> GetNoStockProducts()
        {
            IQueryable<Product> data = _productReadRepository.Table
                .Where(p => p.Stock == 0)
                .Include(p => p.Category)
                .Include(p => p.ProductImageFiles)
                .OrderByDescending(p => p.CreatedDate);

            var products = await data.Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.Description,
                p.Barcode,
                Category = p.Category.Name,
                CategoryId = p.Category.Id,
                CreatedDate = p.CreatedDate.ToString("dd.MM.yyyy"),
                p.Active,
                ImagePath = p.ProductImageFiles.FirstOrDefault(pif => pif.Showcase).Path
            }).ToListAsync();

            return Ok(products);
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Public, ActionType = ActionType.Writing, Definition = "User Send Message")]
        public async Task<IActionResult> UserSendMessage([FromBody]ContactUs model)
        {
            await _mailService.SendContactUsMail(model.Type, model.NameSurname, model.PhoneNumber, model.Message);
            return Ok();
        }
    } 
}
