using MilkStore_BAL.ModelViews.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> ValidateItemInCart(List<OrderProductDto> cartItems);
        Task<string> CreateOrder(List<OrderProductDto> cartItems, int? voucherId, int exchangedPoint);
        Task<bool> CheckVoucher(int voucherId);
        Task<bool> ValidateExchangedPoint(int exchangedPoint, int customerId);
        Task<List<OrderDtoResponse>> Get();
        Task<OrderDtoResponse?> Get(int id);
        Task<List<OrderDtoResponse>> GetByCustomerId(int customerId, int status);
    }
}
