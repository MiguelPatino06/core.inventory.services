using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TShirt.Inventory.Mobile.ViewModels;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.Inventory.Mobile.Pages.Count
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PList : ContentPage
    {
        private CountServices countServices;
        public PList()
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
            var x = (CountPlan)e.SelectedItem;


            //var result = await countServices.GetById(x.Id);
            //if (result.Any(a => a.TotalProduct > 0))
            //{
            //    var answer = await DisplayAlert("TSHIRT", x.Name + " ya tiene SubConteos, Desea Continuar un nuevo SubConteo?", "SI", "NO");
            //    if (answer)
            //        Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Plan(x.Id));

            //}
            //else
            //    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Plan(x.Id));
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
           Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
        }



    }

   
}
