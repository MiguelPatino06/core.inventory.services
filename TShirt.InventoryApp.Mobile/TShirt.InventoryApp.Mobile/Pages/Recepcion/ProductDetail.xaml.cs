using System;
using System.Collections.Generic;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Recepcion
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetail : ContentPage
    {
       
        public ProductDetail(OrderDetailExtend details)
        {
            InitializeComponent();
            BindingContext = new OrderDetailViewModel(details);
        }

    
        //void OnButtonClicked(object sender, EventArgs args)
        //{

        //    EnteredName.Text = string.Empty;

        //    overlay.IsVisible = true;

        //    EnteredName.Focus();
        //}

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            EnteredName.Text = string.Empty;
            overlay.IsVisible = true;
            EnteredName.Focus();
        }

        private void OnTapGestureRecognizerTapped2(object sender, EventArgs e)
        {

            OrderDetailViewModel mvm = sender as OrderDetailViewModel;


            var arrayCodes = ((OrderDetailViewModel) BindingContext).OrderProduct;

            var list = new List<OrderTShirt>();
            foreach (var item in arrayCodes)
            {
                list.Add(new OrderTShirt() { Code = item });
            }


            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new OrdersPage(list, null));
        }

        private void OnOKButtonClicked(object sender, EventArgs e)
        {
            overlay.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntrQuantity.Focus();

        }
    }

 
}
