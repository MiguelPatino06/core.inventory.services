using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.WarehouseProductTransfer
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ProductsTransfer : ContentPage
  {
    public ProductsTransfer()
    {
      InitializeComponent();
    }

    private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
      Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Warehouses());
    }
  }
}