using System;
using System.Collections.Generic;
using XamarinJwtAuth.ViewModels;
using XamarinJwtAuth.Views;
using Xamarin.Forms;

namespace XamarinJwtAuth
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
