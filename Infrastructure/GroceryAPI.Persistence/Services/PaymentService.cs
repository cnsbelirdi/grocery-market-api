using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.DTOs.Payment;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Policy;

namespace GroceryAPI.Persistence.Services
{
    public class PaymentService : IPaymentService
    {
        readonly IPaymentReadRepository _paymentReadRepository;
        readonly IPaymentWriteRepository _paymentWriteRepository;
        private float _totalIncomeAmount;
        private float _totalExpenseAmount;

        public PaymentService(IPaymentReadRepository paymentReadRepository, IPaymentWriteRepository paymentWriteRepository)
        {
            _paymentReadRepository = paymentReadRepository;
            _paymentWriteRepository = paymentWriteRepository;
            _totalIncomeAmount = GetTotalIncomeAmount().Result;
            _totalExpenseAmount = GetTotalExpenseAmount().Result;
        }
        private async Task<float> GetTotalIncomeAmount()
        {
            IQueryable<IncomePayment> incomePayments = _paymentReadRepository.GetWhere<IncomePayment>(p => p is IncomePayment);
            return await incomePayments.SumAsync(p => p.Amount);
        }

        private async Task<float> GetTotalExpenseAmount()
        {
            IQueryable<ExpensePayment> expensePayments = _paymentReadRepository.GetWhere<ExpensePayment>(p => p is ExpensePayment);
            return await expensePayments.SumAsync(p => p.Amount);
        }

        public float TotalIncomeAmount => _totalIncomeAmount;

        public float TotalExpenseAmount => _totalExpenseAmount;

        public async Task CreatePaymentAsync(CreatePayment createPayment)
        {
            Payment newPayment;

            if (createPayment.Type == "IncomePayment")
            {
                newPayment = new IncomePayment();
            }
            else if (createPayment.Type == "ExpensePayment")
            {
                newPayment = new ExpensePayment();
            }
            else
            {
                throw new ArgumentException("Invalid payment type");
            }

            newPayment.Information = createPayment.Information;
            newPayment.Amount = createPayment.Amount;

            await _paymentWriteRepository.AddAsync(newPayment);
            await _paymentWriteRepository.SaveAsync();
        }

        public async Task<ListPayment> GetAllPaymentsAsync(int page, int size)
        {
            var query = _paymentReadRepository.GetAll(false)
                .OrderByDescending(p => p.CreatedDate);

            IQueryable<Payment> data = null;
            if (page != -1 && size != -1)
                data = query.Skip(page * size).Take(size);
            else
                data = query;

            return new()
            {
                TotalPaymentCount = await query.CountAsync(),
                Payments = data.Select(p => new
                {
                    p.Id,
                    Type = p.GetType().Name,
                    p.Information,
                    p.Amount,
                    CreatedDate = p.CreatedDate.ToString("dd.MM.yyyy"),
                })
            };
        }

        public async Task RemovePaymentAsync(string id)
        {
            await _paymentWriteRepository.RemoveAsync(id);
            await _paymentWriteRepository.SaveAsync();
        }

        public async Task UpdatePaymentAsync(UpdatePayment updatePayment)
        {
            Payment payment = await _paymentReadRepository.GetByIdAsync(updatePayment.Id);

            if (payment == null)
            {
                throw new ArgumentException("Payment not found");
            }

            payment.Information = updatePayment.Information;
            payment.Amount = updatePayment.Amount;

            if (updatePayment.Type != payment.GetType().Name)
            {
                await RemovePaymentAsync(payment.Id.ToString());
                await CreatePaymentAsync(new()
                {
                    Type = updatePayment.Type,
                    Information = updatePayment.Information,
                    Amount = updatePayment.Amount,
                });
            }
            else
            {
                await _paymentWriteRepository.SaveAsync();
            }
        }

        public async Task<SinglePayment> GetPaymentByIdAsync(string id)
        {
            Payment payment = await _paymentReadRepository.GetByIdAsync(id);
            return new()
            {
                Id = payment.Id.ToString(),
                Type = payment.GetType().Name,
                Information = payment.Information,
                Amount = payment.Amount,
                CreatedDate = payment.CreatedDate.ToString("dd.MM.yyyy"),
            };
        }
    }
}
