using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamarinJwtAuth.ViewModels;

namespace XamarinJwtAuth.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            this.BindingContext = new RegisterViewModel();
        }
    }
}
