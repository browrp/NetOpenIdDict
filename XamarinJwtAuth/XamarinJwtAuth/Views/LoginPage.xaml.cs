using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinJwtAuth.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using XamarinJwtAuth.TokenManagement;

namespace XamarinJwtAuth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();

            //((LoginViewModel)this.BindingContext).NotifyTokenInfoEvent += NotifyTokenInfoEventHandler;

            ((LoginViewModel)this.BindingContext).OnLoginResult = (obj) => { HandleOnLoginResult(obj); };

        }

        private async void NotifyTokenInfoEventHandler()
        {
            var messageTitle = ((LoginViewModel)this.BindingContext).AlertMessageTitle;
            var message = ((LoginViewModel)this.BindingContext).AlertMessage;

            if(MainThread.IsMainThread)
            {
                await DisplayAlert(title: messageTitle, message: message, cancel: "Ok");
            }
            else
            {
                _ = MainThread.InvokeOnMainThreadAsync(async () => {
                                    await DisplayAlert(title: messageTitle, message: message, cancel: "Ok");
                                    });

             
            }

        }

        private async void HandleOnLoginResult(LoginResult loginResult)
        {

            var title = "Action: " + loginResult.TokenRequestResult.ToString();
            var message = ((LoginViewModel)this.BindingContext).AlertMessage;

            _ =  MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert(title: title,
                                   message: message,
                                   cancel: "Ok");
            });
        }


        private void HandleBindingContextChanged(object sender, EventArgs e)
        {
            
            throw new NotImplementedException();
        }
    }
}