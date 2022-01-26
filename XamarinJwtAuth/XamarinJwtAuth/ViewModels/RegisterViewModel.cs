using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinJwtAuth.Services;

namespace XamarinJwtAuth.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {

        public RegisterViewModel()
        {
            RegisterUser = new AsyncCommand(OnRegisterUser);
            validPassword = false;

            
        }

        public ICommand RegisterUser { get; }

        private async Task OnRegisterUser()
        {

            // Call into the Registration Service

            // If successful then alert the user and either:
            // Auto Sign in the User
            // Take them to the Sign In Screen

            IsBusy = true;
            RegistrationService svc = new RegistrationService();
            var registrationResult = await svc.RegisterUserAsync(new Models.RegisterUserModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            });

            if (registrationResult.Status == RegisterUserStatus.Success)
            {

            }

            var i = 0;
            i++;
        }



        private bool validPassword;


        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private string passwordConfirm;
        private string phone;




        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set
            {
                
                SetProperty(ref password, value);
                if (!String.IsNullOrEmpty(password) && password == passwordConfirm)
                {
                    ValidPassword = true;   
                }
                else
                {
                    ValidPassword = false;
                }
            }
        }

        public string PasswordConfirm
        {
            get => passwordConfirm;
            set
            {
                SetProperty(ref passwordConfirm, value);
                if (!String.IsNullOrEmpty(password) && password == passwordConfirm)
                {
                    ValidPassword = true;
                }
                else
                {
                    ValidPassword = false;
                }
            }
        }
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public bool ValidPassword {
            get => validPassword;
            set => SetProperty(ref validPassword, value);
        }
    }
}
