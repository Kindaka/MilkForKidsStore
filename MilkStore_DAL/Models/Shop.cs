using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Models
{
    public partial class Shop
    {
        public Shop()
        {
            Products = new HashSet<Product>();
        }

        public int ShopId { get; set; }
        public int AccountId { get; set; }
        public string ShopName { get; set; } = null!;
        public string ShopDetail { get; set; } = null!;
        public string ShopAddress { get; set; } = null!;
        public DateTime ShopStartTime { get; set; }
        public DateTime ShopEndTime { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
