using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinJwtAuth.Views
{
    public partial class WebAuthenticatorPage : ContentPage
    {
        public WebAuthenticatorPage()
        {
            InitializeComponent();
        }

        string codeVerifier;



        /*
        string url = identityService.CreateAuthorizationRequest();
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(url), new Uri(Constants.RedirectUri));

        */
        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                //using the method from the one article
                //var authResult = await WebAuthenticator.AuthenticateAsync(
                //new Uri(CreateAuthorizationRequest()),
                //new Uri("xamarinWebAuth://"));

                //attempting to use the Okto documentation but modiied to work with OpenIdDict
                var loginService = new LoginService();
                var firstLoginUrl = CreateAuthorizationRequest();
                var secondLoginUrl = loginService.BuildAuthenticationUrl();


                var authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri(loginService.BuildAuthenticationUrl()),
                    new Uri("xamarinWebAuth://"));

                var accessToken = authResult?.AccessToken;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception {ex.GetType()} occurred: {ex.Message}, {ex.InnerException}");
            }
            
        }



        public string CreateAuthorizationRequest()
        {
            // Create URI to authorization endpoint
            //var authorizeRequest = new RequestUrl(Constants.AuthorizeUri);
            
            // Dictionary with values for the authorize request
            var dic = new Dictionary<string, string>();
            dic.Add("client_id", Constants.ClientId);
            //dic.Add("client_secret", Constants.ClientSecret);
            dic.Add("response_type", "code");
            //dic.Add("scope", Constants.Scope);
            dic.Add("redirect_uri", Constants.RedirectUri);
            dic.Add("nonce", Guid.NewGuid().ToString("N"));
            dic.Add("code_challenge", CreateCodeChallenge());
            dic.Add("code_challenge_method", "S256");

            // Add CSRF token to protect against cross-site request forgery attacks.
            var currentCSRFToken = Guid.NewGuid().ToString("N");
            dic.Add("state", currentCSRFToken);

            //var authorizeUri = authorizeRequest.Create(dic);
            var authorizeUri = AddQueryString(Constants.AuthorizeUri, dic);
            Console.WriteLine($"AuthorizeUri: {authorizeUri}");
            return authorizeUri;
        }

        string CreateCodeChallenge()
        {
            string codeChallenge;

            codeVerifier = CryptoRandom.CreateUniqueId();
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                codeChallenge = Base64Url.Encode(challengeBytes);
            }
            return codeChallenge;
        }

        public static string AddQueryString(
    string uri,
    IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (queryString == null)
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurance.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                if (parameter.Value == null) continue;

                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }
    }
}
