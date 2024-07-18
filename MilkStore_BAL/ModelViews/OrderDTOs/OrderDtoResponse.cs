using MilkStore_BAL.ModelViews.OrderDetailDTOs;
using MilkStore_BAL.ModelViews.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.OrderDTOs
{
    public class OrderDtoResponse
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? VoucherId { get; set; }
        public int? ExchangedPoint { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }

        public List<OrderDetailDtoResponse> orderDetails { get; set; } = new List<OrderDetailDtoResponse>();
        public PaymentDtoResponse? paymentDetails { get; set; }
    }
}
