using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MilkStore_BAL.ModelViews.AccountDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserAuthenticatingDtoResponse?> AuthenticateUser(UserAuthenticatingDtoRequest loginInfo)
        {
            try
            {
                UserAuthenticatingDtoResponse response = new UserAuthenticatingDtoResponse();
                string hashedPassword = await HashPassword(loginInfo.Password);
                var account = (await _unitOfWork.AccountRepository.FindAsync(a => a.Email == loginInfo.Email && a.Password == hashedPassword)).FirstOrDefault();
                if (account != null)
                {
                    response = _mapper.Map<UserAuthenticatingDtoResponse>(account);
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateAccountCustomer(UserRegisterDtoRequest newAccount)
        {
            try
            {
                bool status = false;
                newAccount.Password = await HashPassword(newAccount.Password);
                var account = _mapper.Map<Account>(newAccount);
                account.Status = true;
                account.RoleId = 3;
                await _unitOfWork.AccountRepository.AddAsync(account);
                await _unitOfWork.SaveAsync();
                var insertedAccount = (await _unitOfWork.AccountRepository.FindAsync(a => a.Email == newAccount.Email)).FirstOrDefault();
                if (insertedAccount != null)
                {
                    
                    var customer = new Customer
                    {
                        AccountId = insertedAccount.AccountId,
                        UserName = newAccount.UserName,
                        Phone = newAccount.Phone,
                        Address = newAccount.Address,
                        Dob = newAccount.Dob,
                        Point = 0,
                        Status = true
                    };
                    await _unitOfWork.CustomerRepository.AddAsync(customer);
                    await _unitOfWork.SaveAsync();
                    status = true;
                    return status;
                }
                return status;
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

        public async Task<string> GenerateAccessToken(UserAuthenticatingDtoResponse account)
        {
            try
            {
                // Retrieve the customer information based on AccountId
                var customer = (await _unitOfWork.CustomerRepository.GetAsync(c => c.AccountId == account.AccountId)).FirstOrDefault();
                if (customer == null)
                {
                    throw new Exception("Customer not found.");
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var accessClaims = new List<Claim>
        {
            new Claim("AccountId", account.AccountId.ToString()),
            new Claim("CustomerId", customer.CustomerId.ToString()), // Include CustomerId in claims for Chat
            new Claim("RoleId", account.RoleId.ToString())
        };
                var accessExpiration = DateTime.Now.AddMinutes(30);
                var accessJwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], accessClaims, expires: accessExpiration, signingCredentials: credentials);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(accessJwt);
                return accessToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> GetAccountByEmail(string email)
        {
            try
            {
                var account = (await _unitOfWork.AccountRepository.GetAsync(c => c.Email == email)).FirstOrDefault();
                if (account == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> HashPassword(string password)
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
