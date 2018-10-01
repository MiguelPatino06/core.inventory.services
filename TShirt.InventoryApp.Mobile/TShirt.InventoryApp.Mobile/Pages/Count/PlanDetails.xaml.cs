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

namespace TShirt.InventoryApp.Mobile.Pages.Count
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanDetails : ContentPage
    {
        private int idPlan = 0;

        public PlanDetails(int id, string product, string user, string description, string barCodeProduct, string nameProduct)
        {
            idPlan = id;
            InitializeComponent();
            BindingContext = new CountDetailViewModel(id, product, user, description, barCodeProduct, nameProduct);
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {     
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Plan(idPlan));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntrQuantity.Focus();
        }

    }

   
}
