﻿using GroceryAPI.Domain.Entities.Common;
using GroceryAPI.Domain.Entities.Identity;

namespace GroceryAPI.Domain.Entities
{
    public class Basket : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Order Order { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}