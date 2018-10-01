using System;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.Output
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class Detail : ContentPage
  {
    public Detail(int outputId)
    {
      InitializeComponent();
      BindingContext = new OutputDetailViewModel(outputId);
    }

    private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
      Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
    }

  }
}