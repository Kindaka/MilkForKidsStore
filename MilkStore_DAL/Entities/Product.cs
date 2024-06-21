using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public int? VoucherId { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductInfor { get; set; } = null!;
        public double ProductPrice { get; set; }
        public DateTime ProductQuatity { get; set; }
        public string ImageProduct { get; set; } = null!;

        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual Shop Shop { get; set; } = null!;
        public virtual VoucherOfshop? Voucher { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
