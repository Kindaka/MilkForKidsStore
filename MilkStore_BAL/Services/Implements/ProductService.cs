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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddNewProduct(Product product, List<string> imagePaths)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    bool status = false;
                    var checkCategory = _unitOfWork.ProductCategoryRepository.GetByIDAsync(product.ProductCategoryId);
                    if (checkCategory != null)
                    {
                        await _unitOfWork.ProductRepository.AddAsync(product);
                        await _unitOfWork.SaveAsync();

                        if (imagePaths.Any())
                        {
                            foreach (var imagePath in imagePaths)
                            {
                                if (!String.IsNullOrEmpty(imagePath))
                                {
                                    var image = new ImageProduct
                                    {
                                        ProductId = product.ProductId,
                                        ImageProduct1 = imagePath
                                    };
                                    await _unitOfWork.ImageProductRepository.AddAsync(image);
                                    await _unitOfWork.SaveAsync();
                                }
                            }
                        }

                        status = true;
                        await transaction.CommitAsync();
                        return status;
                    }
                    else
                    {
                        return status;
                    }

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<(bool checkDelete, List<string>? oldImagePaths)> DeleteProduct(int id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var checkProduct = await _unitOfWork.ProductRepository.GetByIDAsync(id);
                    if (checkProduct != null)
                    {
                        var Images = (await _unitOfWork.ImageProductRepository.GetAsync(p => p.ProductId == checkProduct.ProductId)).ToList();
                        var currentImagePaths = new List<string>();
                        if (Images.Any())
                        {
                            foreach (var image in Images)
                            {
                                await _unitOfWork.ImageProductRepository.DeleteAsync(image);
                                await _unitOfWork.SaveAsync();
                                currentImagePaths.Add(image.ImageProduct1);
                            }
                        }
                        await _unitOfWork.ProductRepository.DeleteAsync(checkProduct);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return (true, currentImagePaths);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<ProductDtoResponse>> GetAllProducts(int CategoryId)
        {
            try
            {
                var products = new List<Product>();
                if (CategoryId == 0)
                {
                    products = (await _unitOfWork.ProductRepository.GetAsync()).ToList();
                }
                else
                {
                    products = (await _unitOfWork.ProductRepository.GetAsync(p => p.ProductCategoryId == CategoryId)).ToList();
                }
                if (products.Any())
                {
                    List<ProductDtoResponse> list = new List<ProductDtoResponse>();
                    foreach (var product in products)
                    {
                        var productView = _mapper.Map<ProductDtoResponse>(product);
                        var productImages = (await _unitOfWork.ImageProductRepository.GetAsync(p => p.ProductId == product.ProductId)).FirstOrDefault();
                        if (productImages != null)
                        {
                            var image = new ImageProductView
                            {
                                ImageProduct1 = productImages.ImageProduct1
                            };
                            productView.Images.Add(image);
                        }
                        list.Add(productView);
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ProductDtoResponse> GetProductByID(int id)
        {
            try
            {
                var product = (await _unitOfWork.ProductRepository.GetAsync(filter: p => p.ProductId == id, includeProperties: "ProductCategory")).FirstOrDefault();
                if (product != null)
                {
                    var productView = _mapper.Map<ProductDtoResponse>(product);
                    var productImages = await _unitOfWork.ImageProductRepository.GetAsync(p => p.ProductId == product.ProductId);
                    if (productImages.Any())
                    {
                        foreach (var image in productImages)
                        {
                            var imageView = new ImageProductView
                            {
                                ImageProduct1 = image.ImageProduct1
                            };
                            productView.Images.Add(imageView);
                        }
                    }
                    return productView;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ProductDtoResponse>> Search(string searchInput)
        {
            try
            {
                var products = (await _unitOfWork.ProductRepository.FindAsync(p => searchInput != null && p.ProductName.Contains(searchInput))).ToList();
                if (products.Any())
                {
                    List<ProductDtoResponse> list = new List<ProductDtoResponse>();
                    foreach (var product in products)
                    {
                        var productView = _mapper.Map<ProductDtoResponse>(product);
                        var productImages = (await _unitOfWork.ImageProductRepository.GetAsync(p => p.ProductId == product.ProductId)).FirstOrDefault();
                        if (productImages != null)
                        {
                            var imageView = new ImageProductView
                            {
                                ImageProduct1 = productImages.ImageProduct1
                            };
                            productView.Images.Add(imageView);
                        }
                        list.Add(productView);
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        public async Task<(bool check, List<string>? oldImagePaths)> UpdateProduct(ProductDtoRequest request, List<string> imagePaths, int id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    bool status = false;
                    var checkCategory = _unitOfWork.ProductCategoryRepository.GetByIDAsync(request.ProductCategoryId);
                    if (checkCategory != null)
                    {
                        var checkProduct = await _unitOfWork.ProductRepository.GetByIDAsync(id);
                        if (checkProduct != null)
                        {
                            var product = _mapper.Map(request, checkProduct);
                            await _unitOfWork.ProductRepository.UpdateAsync(product);
                            await _unitOfWork.SaveAsync();
                            var currentImagePaths = new List<string>();

                            var currentImages = await _unitOfWork.ImageProductRepository.GetAsync(p => p.ProductId == checkProduct.ProductId);
                            if (currentImages.Any())
                            {
                                foreach (var image in currentImages)
                                {
                                    await _unitOfWork.ImageProductRepository.DeleteAsync(image);
                                    await _unitOfWork.SaveAsync();
                                    currentImagePaths.Add(image.ImageProduct1);
                                }
                            }

                            if (imagePaths.Any())
                            {
                                foreach (var imagePath in imagePaths)
                                {
                                    if (!String.IsNullOrEmpty(imagePath))
                                    {
                                        var image = new ImageProduct
                                        {
                                            ProductId = checkProduct.ProductId,
                                            ImageProduct1 = imagePath
                                        };
                                        await _unitOfWork.ImageProductRepository.AddAsync(image);
                                        await _unitOfWork.SaveAsync();
                                    }
                                }
                            }

                            status = true;
                            await transaction.CommitAsync();
                            return (status, currentImagePaths);
                        }
                        else
                        {
                            return (status, null);
                        }
                    }
                    else
                    {
                        return (status, null);
                    }

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
