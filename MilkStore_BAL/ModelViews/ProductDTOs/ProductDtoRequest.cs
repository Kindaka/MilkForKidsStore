﻿using MilkStore_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.ProductDTOs
{
    public class ProductDtoRequest
    {
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductInfor { get; set; } = null!;
        public double ProductPrice { get; set; }
        public int ProductQuatity { get; set; }

        public List<ImageProductView> Images { get; set; } = new List<ImageProductView>();
    }
}
