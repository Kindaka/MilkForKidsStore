using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.VoucherOfShopDTOs
{
    public class VoucherOfShopDtoResponseForAdmin
    {
        public int VoucherId { get; set; }
        public double VoucherValue { get; set; }
        public int VoucherQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
    }
}
