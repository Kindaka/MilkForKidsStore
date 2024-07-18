using MilkStore_BAL.ModelViews.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.CartDTOs
{
    public class CartDtoResponse
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int CartQuantity { get; set; }

        public ProductDtoResponse ProductView { get; set; } = new ProductDtoResponse();
    }
}
