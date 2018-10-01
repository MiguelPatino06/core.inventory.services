using System;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Recepcion
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Rct : ContentPage
    {
        public Rct(string ordercode, RctExtendModel rct)
        {
            InitializeComponent();
            BindingContext = new RctViewModel(ordercode, rct);
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Page2());
        }
    }
}
