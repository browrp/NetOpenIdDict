using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xamarin.Essentials;

namespace XamarinJwtAuth.TokenManagement
{
    public class TokenManager : ITokenManager
    {

        private string _bearerToken { get; set; }
        private string _refreshToken { get; set; }
        private long _utcExpiryUnixEpoch { get; set; }
        private DateTime _expiryDateTime { get; set; }
        private string _client { get; set; }
        private string _clientSecret { get; set; }

        private string _baseAddress { get; set; }

        private HttpClient _httpClient;

        
        public TokenManager()
        {
            _client = "postman";
            _clientSecret = "postman-secret";

#if DEBUG
            HttpClientHandler insecureHandler = GetInsecureHandler();
            _httpClient = new HttpClient(insecureHandler);
#else
            HttpClient _httpClient = new HttpClient();
#endif
            // Base Url logic from https://docs.microsoft.com/en-us/xamarin/cross-platform/deploy-test/connect-to-local-web-services
            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5001" : "https://localhost:5001";

            _httpClient.BaseAddress = new Uri(_baseAddress);

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

            /*
            // Temporary bypass cert checks in Android https://stackoverflow.com/questions/58376095/xamarin-java-security-cert-certpathvalidatorexception-trust-anchor-for-certifi
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            // Use a Named Client here with a Polly principal attached, give the user the ability to pass in a
            // previously named HttpClient.  Don't over-engineer this right now.
            var client = new HttpClient(httpClientHandler);
            */

            var request = new HttpRequestMessage(HttpMethod.Post, "connect/token");
            request.Content = new FormUrlEncodedContent(keyValueList);

            
            var response = await _httpClient.SendAsync(request);

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




        public async Task<LoginResult> RefreshToken()
        {
            if (_refreshToken == null)
                return new LoginResult()
                {
                    TokenRequestResult = TokenRequestResult.Fail_NoRefreshToken,
                    Message = "No Refresh Token available"
                };

            var keyValueList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _refreshToken),
                new KeyValuePair<string, string>("client_id", _client),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
            };


            // Temporary bypass cert checks
            // in Android https://stackoverflow.com/questions/58376095/xamarin-java-security-cert-certpathvalidatorexception-trust-anchor-for-certifi
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            // Use a Named Client here with a Polly principal attached, give the user the ability to pass in a
            // previously named HttpClient.  Don't over-engineer this right now.
            var client = new HttpClient(httpClientHandler);

            // http://10.0.2.2
            var request = new HttpRequestMessage(HttpMethod.Post, "https://10.0.2.2:5001/connect/token");
            request.Content = new FormUrlEncodedContent(keyValueList);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Pull off the token, refesh
                var responseText = await response.Content.ReadAsStringAsync();

                var tokenResult = await JsonSerializer.DeserializeAsync<TokenResult>(await response.Content.ReadAsStreamAsync());
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
                var tokenResultError = await JsonSerializer.DeserializeAsync<TokenResultError>(await response.Content.ReadAsStreamAsync());

                return new LoginResult()
                {
                    TokenRequestResult = TokenRequestResult.Fail,
                    Message = $"Failed with error: {tokenResultError.error} With error description: {tokenResultError.error_description} "
                };
            }

        }

        // This method must be in a class in a platform project, even if
        // the HttpClient object is constructed in a shared project.
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }



    }

    /// <summary>
    /// Payload from a Successful Token Request
    /// </summary>
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
        Fail,
        Fail_NoRefreshToken,
        Fail_ServerError,
        Fail_NoInternetConnection,
        Fail_AccountNotVerified,
        Fail_BadUsernameOrPassword
    } 
}


