using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinJwtAuth.Services;
using XamarinJwtAuth.TokenManagement;
using XamarinJwtAuth.Views;

namespace XamarinJwtAuth
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<TokenManager>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
