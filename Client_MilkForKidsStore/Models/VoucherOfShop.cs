using System;
using System.Collections.Generic;

namespace Client_MilkForKidsStore.Models
{
    public partial class VoucherOfShop
    {
        public VoucherOfShop()
        {
            Orders = new HashSet<Order>();
        }

        public int VoucherId { get; set; }
        public double VoucherValue { get; set; }
        public DateTime StartDate { get; set; }
        public int VoucherQuantity { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
