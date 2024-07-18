using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_MilkForKidsStore.DTO.ProductDTOs
{
    public class CategoryDto
    {
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = null!;
    }
}
