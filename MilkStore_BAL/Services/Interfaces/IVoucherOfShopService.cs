using MilkStore_BAL.ModelViews.VoucherOfShopDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IVoucherOfShopService
    {
        Task<List<VoucherOfShopDtoResponse>> Get();
        Task<VoucherOfShopDtoResponse?> Get(int id);
        Task<List<VoucherOfShopDtoResponseForAdmin>> GetByAdmin();
        Task<VoucherOfShopDtoResponseForAdmin?> GetByAdmin(int id);
        Task Post (VoucherOfShopDtoRequest request);
        Task<bool> Put (int id, VoucherOfShopDtoRequest request);
        Task<bool> UpdateStatus(int id, bool status);
    }
}
