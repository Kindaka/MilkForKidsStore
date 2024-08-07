﻿using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = null!;
        public bool ProductCategoryStatus { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
