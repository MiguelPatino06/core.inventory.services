using System;
using System.Linq;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Count
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlantList : ContentPage
    {
        private CountServices countServices;

        public PlantList()
        {
            InitializeComponent();
            countServices = new CountServices();
            BindingContext = new CountViewModel(null);
        }

        private async void ListCount_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CountViewModel viewModel = sender as CountViewModel;
            if (e.SelectedItem == null)
            {
                return;
            }
            var x = (CountPlan) e.SelectedItem;


            var result = await countServices.GetById(x.Id);
            if (result.Any(a=> a.TotalProduct > 0))
            {
                var answer = await DisplayAlert("TSHIRT", x.Name + " ya tiene SubConteos, Desea Continuar un nuevo SubConteo?", "SI", "NO");
                if(answer)
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Plan(x.Id));

            }  
            else
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Plan(x.Id));
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Page2());
        }
    }



  
}
