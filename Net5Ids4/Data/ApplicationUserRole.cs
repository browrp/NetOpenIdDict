using System;
using Microsoft.AspNetCore.Identity;

namespace NetOpenIdDict.Data
{
   public class ApplicationUserRole : IdentityUserRole<int>
   {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
   }
    
}
