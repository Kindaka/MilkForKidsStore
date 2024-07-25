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
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorizeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool isMatchedCustomer, bool isAuthorizedAccount)> CheckAuthorizeByAccountId(int userAccountId, int accountId)
        {
            try
            {
                bool isAuthorizedAccount = false;
                bool isMatchedCustomer = false;
                var account = (await _unitOfWork.AccountRepository.GetByIDAsync(userAccountId));
                if (account != null)
                {
                    if (account.AccountId == accountId)
                    {
                        isMatchedCustomer = true;
                    }
                }
                var accountJwt = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                if (accountJwt != null)
                {
                    if (accountJwt.RoleId == 1 || accountJwt.RoleId == 2)
                    {
                        isAuthorizedAccount = true;
                    }
                }
                return (isMatchedCustomer, isAuthorizedAccount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckAuthorizeByCartId(int cartId, int customerId)
        {
            try
            {
                var cart = (await _unitOfWork.CartRepository.GetByIDAsync(cartId));
                if (cart != null)
                {
                    if (cart.CustomerId == customerId)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(bool isMatchedCustomer, bool isAuthorizedAccount)> CheckAuthorizeByCustomerId(int customerId, int accountId)
        {
            try
            {
                bool isAuthorizedAccount = false;
                bool isMatchedCustomer = false;
                var account = (await _unitOfWork.CustomerRepository.GetByIDAsync(customerId));
                if (account != null)
                {
                    if (account.AccountId == accountId)
                    {
                        isMatchedCustomer = true;
                    }
                }
                var accountJwt = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                if (accountJwt != null)
                {
                    if (accountJwt.RoleId == 1 || accountJwt.RoleId == 2)
                    {
                        isAuthorizedAccount = true;
                    }
                }
                return (isMatchedCustomer, isAuthorizedAccount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckAuthorizeByFeedbackId(int feedbackId, int customerId)
        {
            try
            {
                var feedback = (await _unitOfWork.FeedbackRepository.GetByIDAsync(feedbackId));
                if (feedback != null)
                {
                    if (feedback.CustomerId == customerId)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(bool isMatchedCustomer, bool isAuthorizedAccount)> CheckAuthorizeByOrderId(int orderId, int accountId)
        {
            try
            {
                bool isAuthorizedAccount = false;
                bool isMatchedCustomer = false;
                var accountJwt = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                if (accountJwt != null)
                {
                    if (accountJwt.RoleId == 1 || accountJwt.RoleId == 2)
                    {
                        isAuthorizedAccount = true;
                    }
                }
                var order = (await _unitOfWork.OrderRepository.GetByIDAsync(orderId));
                if (order != null)
                {
                    var customer = await _unitOfWork.CustomerRepository.GetByIDAsync(order.CustomerId);
                    if (customer.AccountId == accountId)
                    {
                        isMatchedCustomer = true;
                    }
                }
                return (isMatchedCustomer, isAuthorizedAccount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
