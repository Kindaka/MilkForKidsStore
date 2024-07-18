using System;
using System.Collections.Generic;

namespace Client_MilkForKidsStore.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Status { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
