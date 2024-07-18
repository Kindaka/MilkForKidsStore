using System;
using System.Collections.Generic;

namespace Client_MilkForKidsStore.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public string UserName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public int? Point { get; set; }
        public bool Status { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
