using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.Sample
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class List : ContentPage
    {
        public List()
        {
            InitializeComponent();
            BindingContext = new SampleListViewModel();
        }


        private async void ListCount_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //CountViewModel viewModel = sender as CountViewModel;
            if (e.SelectedItem == null)
            {
                return;
            }
            var x = (ViewSampleSumProduct)e.SelectedItem;

            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ClientProduct(x.Id));

        }
    }

   
}
