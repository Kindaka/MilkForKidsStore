using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.FeedbackDTOs
{
    public class FeedbackDtoRequest
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string FeedbackContent { get; set; } = null!;
        public double RateNumber { get; set; }
    }
}
