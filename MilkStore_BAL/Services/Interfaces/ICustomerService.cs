using MilkStore_BAL.ModelViews.CustomerDTOs;
using MilkStore_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto?> GetCustomerByIdAsync(int customerId);
        Task<bool> UpdateCustomerInfoAsync(int customerId, UpdateCustomerDto updateDto);

    }
}
