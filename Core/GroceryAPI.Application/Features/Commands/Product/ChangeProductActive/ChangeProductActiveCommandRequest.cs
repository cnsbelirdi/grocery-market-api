using MediatR;

namespace GroceryAPI.Application.Features.Commands.Product.ChangeProductActive
{
    public class ChangeProductActiveCommandRequest : IRequest<ChangeProductActiveCommandResponse>
    {
        public string Id { get; set; }
    }
}
