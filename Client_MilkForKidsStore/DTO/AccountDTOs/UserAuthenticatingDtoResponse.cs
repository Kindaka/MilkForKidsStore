using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.AccountDTOs
{
    public class UserAuthenticatingDtoResponse
    {
        public int AccountId { get; set; }
        public int RoleId { get; set; }
    }
}
