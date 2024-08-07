﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using MilkStore_BAL.ModelViews.CustomerDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIDAsync(customerId);
                if (customer == null) return null;

                return _mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCustomerByIdAsync");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCustomerInfoAsync(int customerId, UpdateCustomerDto updateDto)
        {
            try
            {
                var existingCustomer = await _unitOfWork.CustomerRepository.GetByIDAsync(customerId);
                if (existingCustomer == null) return false;

                _mapper.Map(updateDto, existingCustomer);
               await _unitOfWork.CustomerRepository.UpdateAsync(existingCustomer);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCustomerInfoAsync");
                throw new Exception(ex.Message);
            }
        }
    }
}
