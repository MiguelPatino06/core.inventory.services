using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.WarehouseProductTransfer
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class List : ContentPage
  {
    public List()
    {
      InitializeComponent();
      BindingContext = new SearchProductTransferViewModel();
    }

    private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
      Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
    }
  }
}