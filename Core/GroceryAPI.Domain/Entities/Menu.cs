﻿using GroceryAPI.Domain.Entities.Common;

namespace GroceryAPI.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
