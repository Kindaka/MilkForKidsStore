using MilkStore_BAL.ModelViews.AccountDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserAuthenticatingDtoResponse?> AuthenticateUser(UserAuthenticatingDtoRequest loginInfo);

        Task<string> GenerateAccessToken(UserAuthenticatingDtoResponse account);

        Task<bool> GetAccountByEmail(string email);

        Task<bool> CreateAccountCustomer(UserRegisterDtoRequest request);
    }
}
