namespace GroceryAPI.Application.Features.Queries.Product.GetProductByBarcode
{
    public class GetProductByBarcodeQueryResponse
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
