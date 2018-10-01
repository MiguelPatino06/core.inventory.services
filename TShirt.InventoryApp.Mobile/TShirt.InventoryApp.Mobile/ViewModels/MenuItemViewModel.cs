using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Pages;
using TShirt.InventoryApp.Mobile.Pages.Count;
using TShirt.InventoryApp.Mobile.Pages.Recepcion;
using TShirt.InventoryApp.Mobile.Pages.Transaction.ProductChange;
using TShirt.InventoryApp.Mobile.Pages.Transaction.WarehouseProductTransfer;
using Xamarin.Forms;
using Menu = TShirt.InventoryApp.Mobile.Pages.Transaction.Menu;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
  public class MenuItemViewModel
  {

    public int Id { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }
    public string Page { get; set; }

    public ICommand NavigateCommand
    {
      get { return new RelayCommand(Navigate); }
    }

    private void Navigate()
    {

      switch (Id)
      {
        case 1:
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Search());
          break;
        case 2:
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlantList());
          break;
        case 3:
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
          break;
        case 4: //Transferencia entre Bodegas
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.WarehouseProductTransfer.SubMenu());
          break;
        case 5: //Salida a Producción
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Output.SubMenu());
          break;
        case 6: // Cambio Producto
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ClientProduct());
          break;
        case 7: // "Muestra":
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Sample.SubMenu());
          break;
        case 8: // Muestra -Crear Solicitud
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Sample.ClientProduct(null));
          break;
        case 9: // Muestra Lista
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Sample.List());
          break;
        case 10: // Cambio Producto -Crear Solicitud
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.ProductChange.ClientProduct());
            break;
        case 11: // Cambio Producto - Lista
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.ProductChange.List());
            break;
        case 12: // Transferencia entre Bodegas - Crear Solicitud
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.WarehouseProductTransfer.Warehouses());
          break;
        case 13: // Transferencia entre Bodegas - Lista
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.WarehouseProductTransfer.List());
          break;
        case 14: // Salida a Producción - Crear Solicitud
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Output.OutputProducts());
          break;
        case 15: // Salida a Producción - Lista
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Output.List());
          break;
        case 16: // Exit App
                    Application.Current.MainPage.Navigation.RemovePage(new Page2());
                    //Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Output.List());
            break;
                default:
          break;
      }
    }
  }
}
