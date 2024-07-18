using AutoMapper;
using MilkStore_BAL.ModelViews.ProductDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateCategory(CategoryDto request)
        {
            try
            {
                var category = _mapper.Map<ProductCategory>(request);
                await _unitOfWork.ProductCategoryRepository.AddAsync(category);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task DeleteCategory(int CategoryId)
        //{
        //    try
        //    {
        //        var category = (await _unitOfWork.ProductCategoryRepository.FindAsync(c => c.ProductCategoryId == CategoryId)).FirstOrDefault();
        //        if (category == null)
        //        {
        //            throw new Exception("No category match this id");
        //        }
        //        else
        //        {
        //            await _unitOfWork.ProductCategoryRepository.DeleteAsync(category);
        //            await _unitOfWork.SaveAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            try
            {
                var categoryes = await _unitOfWork.ProductCategoryRepository.GetAllAsync(c => c.ProductCategoryStatus == true);
                if (categoryes.Count() == 0)
                {
                    return null;
                }
                List<CategoryDto> categoryViews = new List<CategoryDto>();
                foreach (var Type in categoryes)
                {
                    var categoryView = _mapper.Map<CategoryDto>(Type);
                    categoryViews.Add(categoryView);
                }
                return categoryViews;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            try
            {
                var category = (await _unitOfWork.ProductCategoryRepository.GetAllAsync(filter: c => c.ProductCategoryStatus == true && c.ProductCategoryId == id)).FirstOrDefault();
                if (category == null)
                {
                    return null;
                }
                var categoryView = _mapper.Map<CategoryDto>(category);
                return categoryView;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateCategory(int CategoryId, CategoryDto request)
        {
            try
            {
                var category = (await _unitOfWork.ProductCategoryRepository.FindAsync(c => c.ProductCategoryId == CategoryId)).FirstOrDefault();
                if (category == null)
                {
                    throw new Exception("No category match this id");
                }
                else
                {
                    category.ProductCategoryName = request.ProductCategoryName;
                    await _unitOfWork.ProductCategoryRepository.UpdateAsync(category);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
