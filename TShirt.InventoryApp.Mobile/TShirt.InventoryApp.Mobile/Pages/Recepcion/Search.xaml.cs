using System;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Recepcion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        public Search()
        {
            InitializeComponent();
           
            BindingContext = new SearchProviderOrderViewModel();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {           
            SearchProviderOrderViewModel mvm = sender as  SearchProviderOrderViewModel;
            if (e.SelectedItem == null)
            {
                return;
            }
            var x = (Provider)e.SelectedItem;
            // CODE SHOW MESSAGE ALERT
            //DisplayAlert("TShirt Inventory", "El Proveedor " + x.Name + ", no tiene O/C", "OK");
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new OderList(x.Code, x.Name));
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Page2());
        }
    }
}
