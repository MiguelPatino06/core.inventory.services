using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Count
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Plan : ContentPage
    {
        public Plan(int id)
        {
            InitializeComponent();
            BindingContext = new CountViewModel(id);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entBarcode.Focus();
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlantList());
        }
    }
}
