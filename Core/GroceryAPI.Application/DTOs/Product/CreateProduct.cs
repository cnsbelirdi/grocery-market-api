﻿namespace GroceryAPI.Application.DTOs.Product
{
    public class CreateProduct
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public string? CategoryId { get; set; }
    }
}
