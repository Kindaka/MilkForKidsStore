using System;
using System.Collections.Generic;

namespace Client_MilkForKidsStore.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string FeedbackContent { get; set; } = null!;
        public double RateNumber { get; set; }
        public bool Status { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
