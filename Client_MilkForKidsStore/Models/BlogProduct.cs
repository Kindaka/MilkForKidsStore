using System;
using System.Collections.Generic;

namespace Client_MilkForKidsStore.Models
{
    public partial class BlogProduct
    {
        public int BlogProductId { get; set; }
        public int BlogId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }

        public virtual Blog Blog { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
