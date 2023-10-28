using GroceryAPI.Domain.Entities;

namespace GroceryAPI.Application.DTOs.Product
{
    public class SingleProduct
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
