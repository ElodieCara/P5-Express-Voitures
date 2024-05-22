using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ExpressVoitures.Data
{ 
    public class User : IdentityUser
    {
        public User() : base()
        {
            this.Repairs = new HashSet<Repair>();
            this.Sales = new HashSet<Sale>();
        }

        public required string Username { get; set; }
        public virtual ICollection<Repair> Repairs { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}