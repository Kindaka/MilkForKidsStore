using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; } = null!;
        public string BlogContent { get; set; } = null!;
        public string? BlogImage { get; set; }
        public bool Status { get; set; }

        public virtual BlogProduct? BlogProduct { get; set; }
    }
}
