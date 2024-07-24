using MilkStore_BAL.ModelViews.BlogDTOs;
using MilkStore_BAL.ModelViews.FeedbackDTOs;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IList<BlogDtoResponse>> GetAllBlog();

        Task<IList<BlogDtoResponse>> GetAllBlogByBlogId(int BlogId);
        Task<int> ValidateProductOfBolg(BlogProductDto blogItems);

        Task<string> CreateBlog(BlogProductDto blogItems);


    }
}
