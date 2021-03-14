using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NetOpenIdDict.Data
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
