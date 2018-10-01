using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.ProductChange
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubMenu : ContentPage
    {
        public SubMenu()
        {
            InitializeComponent();
            BindingContext = new SubMenuViewModelProductChange();
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView) sender).SelectedItem = null;
    }

    public class SubMenuViewModelProductChange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public SubMenuViewModelProductChange()
        {
            LoadMenu();
        }

        public void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

            Menu.Add(new MenuItemViewModel
            {
                Id = 10,
                Icon = "Images/add.png",
                Name = "Crear Solicitud",
                Page = "Pages/Recepcion/Recepcion"
            });
            Menu.Add(new MenuItemViewModel
            {
                Id = 11,
                Icon = "Images/list.png",
                Name = "Ver Solicitud",
                Page = "Pages/Count/PlanList"
            });

        }
    }



}
