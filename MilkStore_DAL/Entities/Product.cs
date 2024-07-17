using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class Product
    {
        public Product()
        {
            BlogProducts = new HashSet<BlogProduct>();
            Carts = new HashSet<Cart>();
            ImageProducts = new HashSet<ImageProduct>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductInfor { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public int ProductQuatity { get; set; }

        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual ICollection<BlogProduct> BlogProducts { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ImageProduct> ImageProducts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
