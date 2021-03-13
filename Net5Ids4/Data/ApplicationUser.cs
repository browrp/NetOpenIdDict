using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NetOpenIdDict.Data
{
    public class ApplicationUser : IdentityUser<int>
    {

        /// <summary>
        /// First Name of user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// IsActive Allows us to determine programatically if the user is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        //Added and didn't realize that ASP.Net Core Identity already gave me a Phone Number Field
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Surrogate Key for cross database useage.
        /// Value is automatically created/populated by the database.
        /// Added for developer convienence.
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Date & Time User record was created.
        /// Managed by database added for developer convienence.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Date & Time user record last updated.
        /// Managed by database, added for developer convienence.
        /// </summary>
        public DateTime LastUpdate { get; set; }


        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<int>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
