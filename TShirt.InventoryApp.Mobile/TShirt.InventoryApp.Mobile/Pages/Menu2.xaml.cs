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

namespace TShirt.InventoryApp.Mobile.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu2 : ContentPage
    {
        public Menu2()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
         => ((ListView)sender).SelectedItem = null;
    }

  
}
