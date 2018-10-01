using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TShirt.Inventory.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.Inventory.Mobile.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();

            ObservableCollection<MenuItemViewModel> menu2 = new ObservableCollection<MenuItemViewModel>();



           List<Test> m2 = new List<Test>();
            var x =new Test();

            x.x1 = "1";
            x.x2 = "Images/Recepcion.png";

            m2.Add(x);

            //m2.Add(new MenuItemViewModel
            //    {
            //    Id = 1,
            //        Icon = "Images/Recepcion.png",
            //        Name = "Recepcion",
            //        Page = "Pages/Recepcion/Search"
            //    });

            //{
            //    new MenuItemViewModel
            //    {
            //        Id = 1,
            //        Icon = "Images/Recepcion.png",
            //        Name = "Recepcion",
            //        Page = "Pages/Recepcion/Search"
            //    }
            //};




            ListMenu.ItemsSource = new ObservableCollection<Test>(m2); // menu2;
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            //Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Config());
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        public string Name { get; set; }

        //"@drawable/Recepcion.png",
        public void LoadMenu()
        {
            MenuItems = new ObservableCollection<MenuItemViewModel>();

            ImageSource.FromFile("@drawable/Logo.png");

            MenuItems.Add(new MenuItemViewModel
            {
                Id = 1,
                Icon = "Images/Recepcion.png",               
                Name = "Recepcion",
                Page = "Pages/Recepcion/Search"
            });
            MenuItems.Add(new MenuItemViewModel
            {
                Id = 2,
                Icon = "@drawable/Count.png",
                Name = "Conteo",
                Page = "Pages/Count/PList"
            });
            MenuItems.Add(new MenuItemViewModel
            {
                Id = 3,
                Icon = "@drawable/Transaction.png",
                Name = "Transacion",
                Page = "Pages/Recepcion/Transaction"
            });
            MenuItems.Add(new MenuItemViewModel
            {
                Id = 16,
                Icon = "@drawable/exit.png",
                Name = "Salir",
                Page = "Pages/Recepcion/Count"
            });
        }


        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
           => ((ListView)sender).SelectedItem = null;

    }

    public class Test
    {
        public string x1 { get; set; }
        public string x2 { get; set; }
    }
        

    
}
