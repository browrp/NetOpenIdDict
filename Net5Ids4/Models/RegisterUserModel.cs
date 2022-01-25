using System;
using System.ComponentModel.DataAnnotations;

namespace NetOpenIdDict.Models
{
    /// <summary>
    /// Model used for the API Account/Register endpoint to sign up for a new account
    /// </summary>
    public class RegisterUserModel
    {
        public RegisterUserModel()
        {
           
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }


        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
