using AutoMapper;
using MilkStore_BAL.ModelViews.AccountDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Implements;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateAccountStaff(UserRegisterDtoRequest newAccount)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        newAccount.Password = await HashPassword(newAccount.Password);
                        var account = _mapper.Map<Account>(newAccount);
                        account.Status = true;
                        account.RoleId = 2;
                        await _unitOfWork.AccountRepository.AddAsync(account);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                var insertedAccount = (await _unitOfWork.AccountRepository.FindAsync(a => a.Email == newAccount.Email)).FirstOrDefault();
                if (insertedAccount != null)
                {
                    await _unitOfWork.AccountRepository.DeleteAsync(insertedAccount);
                    await _unitOfWork.SaveAsync();
                }
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> LockAccount(int accountId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var account = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                    if (account == null)
                    {
                        throw new ArgumentException($"Account not found for account {accountId}");
                    }
                    account.Status = false;

                    await _unitOfWork.AccountRepository.UpdateAsync(account);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (ArgumentException)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> UnlockAccount(int accountId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var account = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                    if (account == null)
                    {
                        throw new ArgumentException($"Account not found for account {accountId}");
                    }
                    account.Status = true;

                    await _unitOfWork.AccountRepository.UpdateAsync(account);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (ArgumentException)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        private async Task<string> HashPassword(string password)
        {
            try
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        stringBuilder.Append(hashBytes[i].ToString("x2"));
                    }

                    return await Task.FromResult(stringBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
