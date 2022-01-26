using System;
using System.Text.Json.Serialization;

namespace XamarinJwtAuth.Models
{
    public class RegisterUserModel
    {
        public RegisterUserModel()
        {

        }
        [JsonPropertyName("firstName")]
        public string FirstName {get;set;}
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }

        public string Password { get; set; }
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        // These are set on the return from the Register User call.
        // ToDo: Make these readonly, if so will they get set from the constructor call?
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("userGuid")]
        public Guid UserGuid { get; set; }
    }
}


//{"id":15,
//"userGuid":"a1429d76-b5bf-4796-9713-1ad9c8c66c5e",
//"firstName":"Robert",
//"lastName":"Brown",
//"email":"robert.brown@uc.edu"}