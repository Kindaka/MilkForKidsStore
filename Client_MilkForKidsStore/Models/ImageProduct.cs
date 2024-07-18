using System;
using System.Collections.Generic;

namespace Client_MilkForKidsStore.Models
{
    public partial class ImageProduct
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageProduct1 { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
