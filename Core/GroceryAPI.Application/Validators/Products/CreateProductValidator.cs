using FluentValidation;
using GroceryAPI.Application.DTOs.Product;

namespace GroceryAPI.Application.Validators.Products
{
    public  class CreateProductValidator: AbstractValidator<CreateProduct>
    {
        public CreateProductValidator() {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please do not leave product name empty!")
                .MaximumLength(150)
                .MinimumLength(5)
                    .WithMessage("Please type product name between 5 and 150 characters!");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                   .WithMessage("Please do not leave product stock empty!")
                .Must(s => s >= 0)
                   .WithMessage("The stock can not be negative!");


            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                   .WithMessage("Please do not leave product price empty!")
                .Must(p => p >= 0)
                   .WithMessage("The price can not be negative!");
        }
    }
}
