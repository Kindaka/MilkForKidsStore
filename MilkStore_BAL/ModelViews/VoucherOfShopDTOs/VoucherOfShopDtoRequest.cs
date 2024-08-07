﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.VoucherOfShopDTOs
{
    public class VoucherOfShopDtoRequest
    {
        [Range(0, 100)]
        public double VoucherValue { get; set; }
        [Range(0, int.MaxValue)]
        public int VoucherQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
