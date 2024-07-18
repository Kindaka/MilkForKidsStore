using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.CartDTOs
{
    public class CartDtoRequest
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int CartQuantity { get; set; }
    }
}
