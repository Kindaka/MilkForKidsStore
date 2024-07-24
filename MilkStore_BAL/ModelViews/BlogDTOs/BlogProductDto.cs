using MilkStore_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.BlogDTOs
{
    public class BlogProductDto : BlogDtoRequest
    {
        public List<int>? productId {  get; set; }
    }
}
