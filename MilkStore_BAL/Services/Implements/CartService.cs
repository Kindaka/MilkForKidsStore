using AutoMapper;
using MilkStore_BAL.ModelViews.CartDTOs;
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
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddToCart(CartDtoRequest request)
        {
            try
            {
                var existedItemInCart = (await _unitOfWork.CartRepository.GetAsync(filter: c => c.ProductId == request.ProductId && c.CustomerId == request.CustomerId, includeProperties: "Product")).FirstOrDefault();
                if (existedItemInCart == null)
                {
                    var cartItem = _mapper.Map<Cart>(request);
                    await _unitOfWork.CartRepository.AddAsync(cartItem);
                    await _unitOfWork.SaveAsync();
                    return true;
                }
                else
                {
                    var totalItem = existedItemInCart.CartQuantity + request.CartQuantity;
                    if (existedItemInCart.Product.ProductQuantity > totalItem)
                    {
                        existedItemInCart.CartQuantity = totalItem;
                        await _unitOfWork.CartRepository.UpdateAsync(existedItemInCart);
                        await _unitOfWork.SaveAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteItemInCart(int id)
        {
            try
            {
                var cartItemToDelete = await _unitOfWork.CartRepository.GetByIDAsync(id);
                if (cartItemToDelete != null)
                {
                    await _unitOfWork.CartRepository.DeleteAsync(cartItemToDelete);
                    await _unitOfWork.SaveAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CartDtoResponse>> GetCartByCustomerId(int CustomerId)
        {
            try
            {
                var response = new List<CartDtoResponse>();
                var cartItems = await _unitOfWork.CartRepository.GetAsync(c => c.CustomerId == CustomerId, includeProperties: "Product");
                foreach (var item in cartItems)
                {
                    var itemView = _mapper.Map<CartDtoResponse>(item);
                    itemView.ProductView = _mapper.Map<ProductDtoResponse>(item.Product);
                    var productImages = (await _unitOfWork.ImageProductRepository.GetAsync(p => p.ProductId == item.Product.ProductId)).FirstOrDefault();
                    if (productImages != null)
                    {
                        var imageView = new ImageProductView
                        {
                            ImageProduct1 = productImages.ImageProduct1
                        };
                        itemView.ProductView.Images.Add(imageView);
                    }
                    response.Add(itemView);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateItemQuantityInCart(int id, int quantity)
        {
            try
            {
                var cartItem = await _unitOfWork.CartRepository.GetByIDAsync(id);
                if (cartItem != null)
                {
                    if (quantity == 0)
                    {
                        await _unitOfWork.CartRepository.DeleteAsync(cartItem);
                        await _unitOfWork.SaveAsync();
                        return 3;
                    }
                    var product = await _unitOfWork.ProductRepository.GetByIDAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        if (product.ProductQuantity < quantity)
                        {
                            return 2;
                        }
                        else
                        {
                            cartItem.CartQuantity = quantity;
                            await _unitOfWork.CartRepository.UpdateAsync(cartItem);
                            await _unitOfWork.SaveAsync();
                            return 1;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
