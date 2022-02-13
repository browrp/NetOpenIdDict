using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityModel.Client;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinJwtAuth.Models;
using XamarinJwtAuth.Services;


namespace XamarinJwtAuth.Views
{
    public partial class WebAuthenticatorPage : ContentPage
    {
        IIdentityService identityService;
        AuthorizeResponse authorizeResponse;

        public WebAuthenticatorPage()
        {
            InitializeComponent();

            identityService = new IdentityService(new RequestProvider());

        }




        /*
        string url = identityService.CreateAuthorizationRequest();
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(url), new Uri(Constants.RedirectUri));

        */
        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string url = identityService.CreateAuthorizationRequest();
                WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(url), new Uri(Constants.RedirectUri));

                string raw = ParseAuthenticatorResult(authResult);
                authorizeResponse = new AuthorizeResponse(raw);
                if (authorizeResponse.IsError)
                {
                    Console.WriteLine("ERROR: {0}", authorizeResponse.Error);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception {ex.GetType()} occurred: {ex.Message}, {ex.InnerException}");
            }
            
        }

        string ParseAuthenticatorResult(WebAuthenticatorResult result)
        {
            string code = result?.Properties["code"];
            string idToken = result?.IdToken?.ToString();

            //Some values are not in the Properties Dictionary updating the code
            //so that we can check for values before pulling them to avoid exceptions.
            //string scope = result?.Properties["scope"];
            //string state = result?.Properties["state"];
            //string sessionState = result?.Properties["session_state"];

            string scope=string.Empty, state=string.Empty, sessionState=string.Empty;
            if(result != null)
            {
                if (result.Properties.ContainsKey("scope"))
                    scope = result?.Properties["scope"];
                else
                    scope = "";

                if (result.Properties.ContainsKey("state"))
                    state = result?.Properties["state"];
                else
                    state = "";

                if (result.Properties.ContainsKey("session_state"))
                    sessionState = result?.Properties["session_state"];
                else
                    sessionState = "";
            }



            return $"{Constants.RedirectUri}#code={code}&id_token={idToken}&scope={scope}&state={state}&session_state={sessionState}";
        }


        async void OnCallAPIButtonClicked(object sender, EventArgs e)
        {
            if (!authorizeResponse.IsError && !String.IsNullOrEmpty(authorizeResponse.Code))
            {
                UserToken userToken = await identityService.GetTokenAsync(authorizeResponse.Code);
                if (!String.IsNullOrEmpty(userToken.AccessToken) )
                {
                    //var content = await identityService.GetAsync($"{Constants.ApiUri}test", userToken.AccessToken);
                    //editor.Text = JArray.Parse(content).ToString();

                    if (MainThread.IsMainThread)
                    {
                        await DisplayAlert(title: "Token", message: userToken.AccessToken, cancel: "Ok");
                    }
                    else
                    {
                        _ = MainThread.InvokeOnMainThreadAsync(async () => {
                            await DisplayAlert(title: "Token", message: userToken.AccessToken, cancel: "Ok");
                        });


                    }
                }
            }
        }


        private async void LogoutButtonClicked(object sender, EventArgs e)
        {
            
            try
            {
                string url = identityService.CreateAuthorizationRequest();
                WebAuthenticatorResult authResult =
                    await WebAuthenticator.AuthenticateAsync(new Uri($"https://localhost:5001/Identity/Account/Logout?returnUrl={Constants.RedirectUri}"),
                    new Uri(Constants.RedirectUri));

                string raw = ParseAuthenticatorResult(authResult);
                authorizeResponse = new AuthorizeResponse(raw);
                if (authorizeResponse.IsError)
                {
                    Console.WriteLine("ERROR: {0}", authorizeResponse.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.GetType()} occurred: {ex.Message}, {ex.InnerException}");
            }

        }






    }
}
