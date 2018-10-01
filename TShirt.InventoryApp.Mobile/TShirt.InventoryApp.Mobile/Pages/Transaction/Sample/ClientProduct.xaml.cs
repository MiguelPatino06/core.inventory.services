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

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.Sample
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientProduct : ContentPage
    {
        public ClientProduct(int? id)

        {
            InitializeComponent();
            BindingContext = new SampleViewModel(id);
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
        }

    }

   
}
