using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Application.Features.Queries.Order.GetOrderByNumber
{
    public class GetOrderByNumberQueryRequest : IRequest<GetOrderByNumberQueryResponse>
    {
        public string Number { get; set; }
    }
}
