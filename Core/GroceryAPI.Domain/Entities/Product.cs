using GroceryAPI.Domain.Entities.Common;

namespace GroceryAPI.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public Category? Category { get; set; }
        public bool Active { get; set; } = true;
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }
        public ICollection<BasketItem>? BasketItems { get; set; }
        
    }
}
