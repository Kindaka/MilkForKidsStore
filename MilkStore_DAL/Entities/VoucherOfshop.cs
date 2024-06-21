using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class VoucherOfshop
    {
        public VoucherOfshop()
        {
            Products = new HashSet<Product>();
        }

        public int VoucherId { get; set; }
        public string VoucherName { get; set; } = null!;
        public double VoucherValue { get; set; }
        public DateTime VoucherStart { get; set; }
        public DateTime VoucherEnd { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
