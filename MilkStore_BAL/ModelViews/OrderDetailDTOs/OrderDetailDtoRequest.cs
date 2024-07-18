﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.OrderDetailDTOs
{
    public class OrderDetailDtoRequest
    {
        public int ProductId { get; set; }
        public int OrderQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
