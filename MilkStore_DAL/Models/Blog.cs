using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Models
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public int AccountId { get; set; }
        public string BlogName { get; set; } = null!;
        public string BlogContent { get; set; } = null!;
        public string BlogImage { get; set; } = null!;
        public bool Status { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
