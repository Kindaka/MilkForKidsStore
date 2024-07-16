using System;
using System.Collections.Generic;

namespace MilkStore_DAL.Entities
{
    public partial class Account
    {
        public Account()
        {
            Customers = new HashSet<Customer>();
        }

        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Status { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
