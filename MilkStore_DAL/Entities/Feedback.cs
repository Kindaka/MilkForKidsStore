using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int AccountId { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; } = null!;
        public double RateNumber { get; set; }
        public bool Status { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
