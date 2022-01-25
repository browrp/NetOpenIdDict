using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinJwtAuth.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));

            LaunchLoginCommand = new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));

            LaunchRegistrationCommand = new Command(async () => await Shell.Current.GoToAsync("//RegistrationPage"));
        }

        public ICommand OpenWebCommand { get; }

        public ICommand LaunchLoginCommand { get; }

        public ICommand LaunchRegistrationCommand { get; }
    }
}