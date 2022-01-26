using System.ComponentModel;
using Xamarin.Forms;
using XamarinJwtAuth.ViewModels;

namespace XamarinJwtAuth.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}