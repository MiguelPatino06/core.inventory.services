using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.ProductChange
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientProduct : ContentPage
    {
        public ClientProduct()
        {
            InitializeComponent();
            BindingContext = new SearchOrderReqViewModel();
        }


        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
        }


        private void OnTapGestureRecognizerTappedSearch(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
        }
    }

   
}
