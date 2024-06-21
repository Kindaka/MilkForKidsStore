using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IAuthorizeService
    {
        Task<(bool isUser, bool isAdmin)> CheckAuthorizeByAccountId(int userAccountId, int accountId);
    }
}
