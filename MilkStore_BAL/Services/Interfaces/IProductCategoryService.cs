using MilkStore_BAL.ModelViews.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task CreateCategory(CategoryDto request);
        Task UpdateCategory(int CategoryId, CategoryDto request);
        Task DeleteCategory(int CategoryId);
    }
}
