using MilkStore_BAL.Services.Interfaces;
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

        public async Task<(bool isUser, bool isAdmin)> CheckAuthorizeByAccountId(int userAccountId, int accountId)
        {
            try
            {
                bool isAdmin = false;
                bool isUser = false;
                var account = (await _unitOfWork.AccountRepository.GetByIDAsync(userAccountId));
                if (account != null)
                {
                    if (account.AccountId == accountId)
                    {
                        isUser = true;
                    }
                }
                var accountJwt = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                if (accountJwt != null)
                {
                    if (accountJwt.RoleId == 1)
                    {
                        isAdmin = true;
                    }
                }
                return (isUser, isAdmin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
