using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace TShirt.InventoryApp.Mobile.Pages.Transaction
{

  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class Menu : ContentPage
  {
    public Menu()
    {
      InitializeComponent();
      BindingContext = new MenuViewModelTransaction();
    }




    void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
      => ((ListView)sender).SelectedItem = null;

    public class MenuViewModelTransaction : INotifyPropertyChanged
    {
      public event PropertyChangedEventHandler PropertyChanged;

      public ObservableCollection<MenuItemViewModel> Menu { get; set; }
      public MenuViewModelTransaction()
      {
        LoadMenu();
      }

      public void LoadMenu()
      {
        Menu = new ObservableCollection<MenuItemViewModel>();

        Menu.Add(new MenuItemViewModel
        {
          Id = 4,
          Icon = "Images/Transaction.png",
          Name = "Transferencia entre Bodegas",
          Page = "Pages/Recepcion/Recepcion"
        });
        Menu.Add(new MenuItemViewModel
        {
          Id = 5,
          Icon = "Images/Transaction.png",
          Name = "Salida a Producción",
          Page = "Pages/Count/PlanList"
        });
        Menu.Add(new MenuItemViewModel
        {
          Id = 6,
          Icon = "Images/Cambio.png",
          Name = "Cambio Producto",
          Page = "Pages/Transaction/ClientProduct"
        });
        Menu.Add(new MenuItemViewModel
        {
          Id = 7,
          Icon = "Images/Sample.png",
          Name = "Muestra",
          Page = "Pages/Sample/ClientProduct"
        });
      }


    }
  }

}
