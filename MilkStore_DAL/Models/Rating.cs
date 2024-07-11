using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Models
{
    public partial class Rating
    {
        public int RateId { get; set; }
        public int AccountId { get; set; }
        public double RateNumber { get; set; }
        public bool Status { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
