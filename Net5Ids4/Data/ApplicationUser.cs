using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Net5Ids4.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public bool IsActive { get; set; }
        public string MobilePhoneNumber { get; set; }

        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<int>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
