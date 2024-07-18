using MilkStore_BAL.ModelViews.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.OrderDetailDTOs
{
    public class OrderDetailDtoResponse
    {
        public int OrderQuantity { get; set; }
        public double ProductPrice { get; set; }

        public ProductDtoResponse? product { get; set; }
    }
}
