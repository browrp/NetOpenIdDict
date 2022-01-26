using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XamarinJwtAuth.Models;

namespace XamarinJwtAuth.Services
{
    public class RegistrationService
    {
        public RegistrationService()
        {


        }

        public async Task<RegisterUserResult> RegisterUserAsync(RegisterUserModel registerUserModel)
        {
            //Setup the HttpClient

            //Call the Registration Endpoing

            //Check the response value

            //If successful raise Success Event


            ////If failed raise Failed Event
            //var keyValueList = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("grant_type", "password"),
            //    new KeyValuePair<string, string>("username", username),
            //    new KeyValuePair<string, string>("password", password),
            //    new KeyValuePair<string, string>("client_id", _client),
            //    new KeyValuePair<string, string>("client_secret", "postman-secret"),
            //    new KeyValuePair<string, string>("scope", "openid offline_access api")
            //};


            // Temporary bypass cert checks in Android https://stackoverflow.com/questions/58376095/xamarin-java-security-cert-certpathvalidatorexception-trust-anchor-for-certifi
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            // Use a Named Client here with a Polly principal attached, give the user the ability to pass in a
            // previously named HttpClient.  Don't over-engineer this right now.
            var client = new HttpClient(httpClientHandler);

            // http://10.0.2.2
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://10.0.2.2:5001/api/account/register",
            client.BaseAddress = new Uri("https://10.0.2.2:5001/");

            var registrationString = new StringContent(JsonSerializer.Serialize(registerUserModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/account/register", registrationString);


            if (response.IsSuccessStatusCode)
            {
                // Pull off the token, refesh
                var responseText = await response.Content.ReadAsStringAsync();

                var registrationResult = await JsonSerializer.DeserializeAsync<RegisterUserModel>(await response.Content.ReadAsStreamAsync());
                ////DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
                ////DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(tokenResult.exp);
                //tokenResult.ExpiresOn = DateTime.UtcNow.AddSeconds(tokenResult.expires_in);

                ////return dateTimeOffset.LocalDateTime;
                //return new LoginResult()
                //{
                //    TokenRequestResult = TokenRequestResult.Success,
                //    Message = $"Token Expiry {tokenResult.expires_in} \n {tokenResult.access_token}"
                //};
                return new RegisterUserResult()
                {
                    Status = RegisterUserStatus.Success,
                    RegisterUserModel = registrationResult,
                    Message = "Registered Successfully"
                };

            }
            else
            {
                var responseText = await response.Content.ReadAsStringAsync();

                return new RegisterUserResult()
                {
                    Status = RegisterUserStatus.Fail_General,
                    RegisterUserModel = registerUserModel,
                    Message = responseText
                };

                //var tokenResultError = await JsonSerializer.DeserializeAsync<TokenResultError>(await response.Content.ReadAsStreamAsync());

                //return new LoginResult()
                //{
                //    TokenRequestResult = TokenRequestResult.Fail,
                //    Message = $"Failed with error: {tokenResultError.error} With error description: {tokenResultError.error_description} "
                //};
                
            }

        }
    }

    /// <summary>
    /// Result of the User Registration. 
    /// </summary>
    public class RegisterUserResult
    {
        /// <summary>
        /// The status of the Registration Call
        /// </summary>
        public RegisterUserStatus Status { get; set; }

        /// <summary>
        /// The User Registration data, if successful this will contain
        /// the UserID and the UserGuidID
        /// </summary>
        public RegisterUserModel RegisterUserModel { get; set; }

        /// <summary>
        /// A message as to the Success or Fail
        /// </summary>
        public String Message { get; set; }
    }
    



    public enum RegisterUserStatus
    {
        Success,
        Fail_General,
        Fail_Server,
        Fail_ExistingUser,
        Fail_PasswordNotComplexEnough
    }

    
}
