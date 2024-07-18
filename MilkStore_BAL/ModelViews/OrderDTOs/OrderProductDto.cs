using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.OrderDTOs
{
    public class OrderProductDto
    {
        public int cartId { get; set; }
        public int customerId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
    }
}
