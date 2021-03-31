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
            await tokenManager.Login(username, password);
        }
    }
}
