using MilkStore_BAL.ModelViews.ProductDTOs;
using MilkStore_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> AddNewProduct(Product product, List<string> imagePaths);

        Task<bool> AddNewProductFireBase(Product product, List<Stream> imageStreams, List<string> imageFileNames);

        Task<List<ProductDtoResponse>> GetAllProducts(int CategoryId);

        Task<ProductDtoResponse> GetProductByID(int id);

        Task<(bool check, List<string>? oldImagePaths)> UpdateProduct(ProductDtoRequest request, List<string> imagePaths, int id);

        Task<List<ProductDtoResponse>> Search(string searchInput);

        //Task<(bool checkDelete, List<string>? oldImagePaths)> DeleteProduct(int id);
        Task<bool> UpdateProductStatusToFalse(int id);
    }
}
