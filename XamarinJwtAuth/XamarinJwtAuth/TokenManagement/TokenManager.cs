using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text.Json;

namespace XamarinJwtAuth.TokenManagement
{
    public class TokenManager
    {

        private string _bearerToken { get; set; }
        private string _refreshToken { get; set; }
        private long _utcExpiryUnixEpoch { get; set; }
        private DateTime _expiryDateTime { get; set; }

        //FixMe: How do I get the settings below in here? Does Xamarin use a config file?!?

        public TokenManager()
        {

        }

        /// <summary>
        /// Preform a Password-Grant authorization
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        public async Task Login(string username, string password, bool rememberMe = true)
        {
            var keyValueList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("client", "CLIENT_ID"),
                new KeyValuePair<string, string>("scopes", "openid, offline_access api")
            };

            // Use a Named Client here with a Polly principal attached, give the user the ability to pass in a
            // previously named HttpClient.  Don't over-engineer this right now.
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/token");
            request.Content = new FormUrlEncodedContent(keyValueList);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Pull off the token, refesh
                var tokenResult = await JsonSerializer.DeserializeAsync<TokenResult>(await response.Content.ReadAsStreamAsync());
                //DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(tokenResult.exp);

                //return dateTimeOffset.LocalDateTime;


            }
            else
            {
                var tokenResultError = await JsonSerializer.DeserializeAsync<TokenResultError>(await response.Content.ReadAsStreamAsync());
            }



        }



    }

    public class TokenResult {
        public string token { get; set; }
        public long exp { get; set; }
    }

    public class TokenResultError
    {
        string error { get; set; }
        string message { get; set; }
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


