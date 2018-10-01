using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Recepcion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OderList : ContentPage
    {
      
        //public OderList(string code)
        public OderList(string code, string provider)
        {
            InitializeComponent();
            this.BindingContext = new OrderProviderViewModel(code, provider);
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Search());
        }

    }
}
