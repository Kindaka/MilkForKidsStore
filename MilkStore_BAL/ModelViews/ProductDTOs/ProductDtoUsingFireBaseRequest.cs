using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.ProductDTOs
{
    public class ProductDtoUsingFireBaseRequest
    {
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductInfor { get; set; } = null!;
        public double ProductPrice { get; set; }
        public int ProductQuatity { get; set; }
        public bool ProductStatus { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}
