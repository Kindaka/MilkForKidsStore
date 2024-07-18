using System;
using System.Collections.Generic;

namespace Client_MilkForKidsStore.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? VoucherId { get; set; }
        public int? ExchangedPoint { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual VoucherOfShop? Voucher { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
