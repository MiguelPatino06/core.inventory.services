using System;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.WarehouseProductTransfer
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class Warehouses : ContentPage
  {
    public Warehouses()
    {
      InitializeComponent();
      BindingContext = new WarehouseProductTransferViewModel();
    }

    private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
      Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
    }

  }
}