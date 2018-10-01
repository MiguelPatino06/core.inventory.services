using System;
using System.Collections.Generic;
using TShirt.InventoryApp.Mobile.Pages.Count;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Recepcion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        //public OrdersPage(string code)  
        public OrdersPage(List<OrderTShirt> codes, List<ViewOrder> orders)
        {
            InitializeComponent();
            this.BindingContext = new OrderViewModel(codes, orders);        
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entBarcode.Focus();
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Search());
        }
    }
}
