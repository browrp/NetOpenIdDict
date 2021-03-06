using System;
using Microsoft.AspNetCore.Identity;

namespace Net5Ids4.Data
{
   public class ApplicationUserRole : IdentityUserRole<int>
   {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
   }
    
}
