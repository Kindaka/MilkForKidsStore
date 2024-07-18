using MilkStore_BAL.ModelViews.AccountDTOs;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> CreateAccountStaff(UserRegisterDtoRequest newAccount);
        Task<bool> LockAccount(int accountId);
        Task<bool> UnlockAccount(int accountId);
    }
}