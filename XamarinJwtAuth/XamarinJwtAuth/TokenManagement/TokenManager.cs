using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XamarinJwtAuth.TokenManagement
{
    public class TokenManager : ITokenManager
    {

        private string _bearerToken { get; set; }
        private string _refreshToken { get; set; }
        private long _utcExpiryUnixEpoch { get; set; }
        private DateTime _expiryDateTime { get; set; }
        private string _client { get; set; }

        //FixMe: How do I get the settings below in here? Does Xamarin use a config file?!?

        public TokenManager()
        {
            _client = "postman";
        }

        /// <summary>
        /// Preform a Password-Grant authorization
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        public async Task<LoginResult> Login(string username, string password, bool rememberMe = true)
        {
            var keyValueList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("client_id", _client),
                new KeyValuePair<string, string>("client_secret", "postman-secret"),
                new KeyValuePair<string, string>("scope", "openid offline_access api")
            };


            // Temporary bypass cert checks in Android https://stackoverflow.com/questions/58376095/xamarin-java-security-cert-certpathvalidatorexception-trust-anchor-for-certifi
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            // Use a Named Client here with a Polly principal attached, give the user the ability to pass in a
            // previously named HttpClient.  Don't over-engineer this right now.
            var client = new HttpClient(httpClientHandler);

            // http://10.0.2.2
            var request = new HttpRequestMessage(HttpMethod.Post, "https://10.0.2.2:5001/connect/token");
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/token");
            request.Content = new FormUrlEncodedContent(keyValueList);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Pull off the token, refesh
                var responseText = await response.Content.ReadAsStringAsync();

                var tokenResult = await JsonSerializer.DeserializeAsync<TokenResult>( await response.Content.ReadAsStreamAsync() );
                //DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
                //DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(tokenResult.exp);
                tokenResult.ExpiresOn = DateTime.UtcNow.AddSeconds(tokenResult.expires_in);

                //return dateTimeOffset.LocalDateTime;
                return new LoginResult()
                {
                    TokenRequestResult = TokenRequestResult.Success,
                    Message = $"Token Expiry {tokenResult.expires_in} \n {tokenResult.access_token}"
                };
                

            }
            else
            {
                var tokenResultError = await JsonSerializer.DeserializeAsync<TokenResultError>( await response.Content.ReadAsStreamAsync() );

                return new LoginResult()
                {
                    TokenRequestResult = TokenRequestResult.Fail,
                    Message = $"Failed with error: {tokenResultError.error} With error description: {tokenResultError.error_description} "
                };
            }


        }



    }
    
    public class TokenResult
    {
        [JsonPropertyName("access_token")]
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }

        public DateTime ExpiresOn { get; set; }
    }

    public class TokenResultError
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string error_uri { get; set; }
    }
    

    public class LoginResult
    {
        public TokenRequestResult TokenRequestResult { get; set; }
        public string Message { get; set; }
    }


    public enum TokenRequestResult
    {
        Success,
        Fail
    } 
}


