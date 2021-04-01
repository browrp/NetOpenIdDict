using XamarinJwtAuth.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinJwtAuth.TokenManagement;
namespace XamarinJwtAuth.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public Command LoginWithJwtCommand { get; }

        // Create an event to call an event in the view.
        // http://jesseliberty.com/2018/07/16/mvvm-ping-pong/
        public  Action NotifyTokenInfoEvent;    // Doesn't work with paramters Using the Alert with Binding for data.
        private string alertMessageTitle;
        private string alertMessage;

        public string AlertMessageTitle
        {
            get => alertMessageTitle;
            set => SetProperty(ref alertMessageTitle, value);
        }
        public string AlertMessage
        {
            get => alertMessage;
            set => SetProperty(ref alertMessage, value);
        }



        private string username;
        private string password;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            LoginWithJwtCommand = new Command(OnLoginWithJwtCommandClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnLoginWithJwtCommandClicked(object obj)
        {
            var tokenManager = DependencyService.Get<ITokenManager>();
            var loginResponse = await tokenManager.Login(username, password);

            if (loginResponse.TokenRequestResult == TokenRequestResult.Fail)
            {
                this.AlertMessageTitle = "Token Request Failed";
            }
            else
            {
                this.AlertMessageTitle = "Token Request Success!";
            }
            this.AlertMessage = loginResponse.Message;
            NotifyTokenInfoEvent?.Invoke();
            

        }
    }
}
