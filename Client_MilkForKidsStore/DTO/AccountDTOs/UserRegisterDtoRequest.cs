using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.AccountDTOs
{
    public class UserRegisterDtoRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
    }
}
