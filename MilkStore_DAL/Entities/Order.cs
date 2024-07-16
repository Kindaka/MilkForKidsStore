using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? VoucherId { get; set; }
        public int? ExchangedPoint { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual VoucherOfShop? Voucher { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
