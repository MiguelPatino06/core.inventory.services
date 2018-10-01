using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            LoadMenu();
        }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

            Menu.Add( new MenuItemViewModel
            {
                Icon = "Images/Recepcion.png",
                Name ="Recepcion",
                Page = "Pages/Recepcion/Recepcion"
            });
            Menu.Add(new MenuItemViewModel
            {
                Icon = "Images/Count.png",
                Name = "Conteo",
                Page = "Pages/Count/PlanList"
            });
            Menu.Add(new MenuItemViewModel
            {
                Icon = "Images/Transaction.png",
                Name = "Transacion",
                Page = "Pages/Recepcion/Transaction"
            });
            Menu.Add(new MenuItemViewModel
            {
                Icon = "Images/Recepcion.png",
                Name = "Reportes",
                Page = "Pages/Recepcion/Search"
            });
        }
    }
}
