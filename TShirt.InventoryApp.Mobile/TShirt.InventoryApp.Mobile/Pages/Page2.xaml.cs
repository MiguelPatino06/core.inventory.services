using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Pages.Recepcion;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        public Page2()
        {
            InitializeComponent();
            LoadMenu();
            ListMenu.ItemsSource = Menu;
        }


        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Config());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public string Name { get; set; }

        public void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

            ImageSource.FromFile("Images/Logo.png");

            Menu.Add(new MenuItemViewModel
            {
                Id = 1,
                Icon = "Images/Recepcion.png",
                Name = "Recepcion",
                Page = "Pages/Recepcion/Search"
            });
            Menu.Add(new MenuItemViewModel
            {
                Id = 2,
                Icon = "Images/Count.png",
                Name = "Conteo",
                Page = "Pages/Count/PlanList"
            });
            Menu.Add(new MenuItemViewModel
            {
                Id = 3,
                Icon = "Images/Transaction.png",
                Name = "Transacion",
                Page = "Pages/Recepcion/Transaction"
            });
            Menu.Add(new MenuItemViewModel
            {
                Id = 16,
                Icon = "Images/exit.png",
                Name = "Salir",
                Page = "Pages/Recepcion/Count"
            });
        }


        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
           => ((ListView)sender).SelectedItem = null;
    }

   
}
