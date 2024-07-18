using MilkStore_BAL.ModelViews.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface ICartService
    {
        Task<bool> AddToCart(CartDtoRequest request);

        Task<List<CartDtoResponse>> GetCartByCustomerId(int CustomerId);

        Task<bool> DeleteItemInCart(int id);

        Task<int> UpdateItemQuantityInCart(int id, int quantity);
    }
}
