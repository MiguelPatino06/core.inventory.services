using System;
using TShirt.InventoryApp.Mobile.Controls;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.Output
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class OutputProducts : ContentPage
  {
    public OutputProducts()
    {
      InitializeComponent();
      BindingContext = new OutputProductsViewModel();
    }

    private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
      Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new SubMenu());
    }

  }
}