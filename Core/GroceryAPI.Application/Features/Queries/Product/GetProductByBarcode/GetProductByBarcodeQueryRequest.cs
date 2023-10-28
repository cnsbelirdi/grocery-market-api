using MediatR;

namespace GroceryAPI.Application.Features.Queries.Product.GetProductByBarcode
{
    public class GetProductByBarcodeQueryRequest : IRequest<GetProductByBarcodeQueryResponse>
    {
        public string Barcode { get; set; }
    }
}
