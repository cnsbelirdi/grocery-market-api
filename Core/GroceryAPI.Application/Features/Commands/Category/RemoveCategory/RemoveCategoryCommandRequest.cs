using MediatR;

namespace GroceryAPI.Application.Features.Commands.Category.RemoveCategory
{
    public class RemoveCategoryCommandRequest : IRequest<RemoveCategoryCommandResponse>
    {
        public string Id { get; set; }
    }
}
