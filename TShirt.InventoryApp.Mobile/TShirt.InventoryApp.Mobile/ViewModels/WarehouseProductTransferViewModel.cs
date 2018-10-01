using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TShirt.InventoryApp.Mobile.Pages.Transaction;
using TShirt.InventoryApp.Mobile.Pages.Transaction.WarehouseProductTransfer;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Services;
using Xamarin.Forms;
using static TShirt.InventoryApp.Mobile.Infrastructure.EnumTShirt;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
  public class WarehouseProductTransferViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private ProductTransferServices _productTransferServices;
    private WarehouseServices _warehouseServices;
    private XmlServices _xmlServices;

    public WarehouseProductTransferViewModel()
    {
      _warehouseServices = new WarehouseServices();
      _productTransferServices = new ProductTransferServices();
      _xmlServices = new XmlServices();
      ProductTransferCollection = new ObservableCollection<ProductToTransfer>();
      LoadWarehouses();
    }

    #region Properties

    private ObservableCollection<string> _warehouseCollection;
    public ObservableCollection<string> WarehouseCollection
    {
      get { return _warehouseCollection; }
      set { _warehouseCollection = value; RaiseOnPropertyChange(); }
    }

    private ObservableCollection<ProductToTransfer> _productTransferCollection;
    public ObservableCollection<ProductToTransfer> ProductTransferCollection
    {
      get { return _productTransferCollection; }
      set
      {
        _productTransferCollection = value;
        HeightList = (_productTransferCollection.Count * 45) + (_productTransferCollection.Count * 5);
        RaiseOnPropertyChange();
      }
    }

    private int _heightList;
    public int HeightList
    {
      get { return _heightList; }
      set { _heightList = value; RaiseOnPropertyChange(); }
    }

    private int _widthComment;
    public int WidthComment
    {
      get { return _widthComment; }
      set { _widthComment = value; RaiseOnPropertyChange(); }
    }

    private string _warehouseOriginSelect;
    public string WarehouseOriginSelect
    {
      get { return _warehouseOriginSelect; }
      set { _warehouseOriginSelect = value; RaiseOnPropertyChange(); }
    }

    private string _warehouseDestinySelect;
    public string WarehouseDestinySelect
    {
      get { return _warehouseDestinySelect; }
      set { _warehouseDestinySelect = value; RaiseOnPropertyChange(); }
    }

    private string _observation;
    public string Observation
    {
      get { return _observation; }
      set { _observation = value; RaiseOnPropertyChange(); }
    }

    private string _productCode;
    public string ProductCode
    {
      get { return _productCode; }
      set { _productCode = value; RaiseOnPropertyChange(); }
    }

    private string _quantity;
    public string Quantity
    {
      get { return _quantity; }
      set { _quantity = value; RaiseOnPropertyChange(); }
    }

    private string _productName;
    public string ProductName
    {
      get { return _productName; }
      set { _productName = value; RaiseOnPropertyChange(); }
    }

    private WarehouseProduct warehouseProduct { get; set; }

    private List<Warehouse> warehouses { get; set; }

    #endregion

    #region METHODS

    private async void LoadWarehouses()
    {
      var result = await _warehouseServices.GetListWarehouse();
      if (result != null)
      {
        warehouses = result;
        WarehouseCollection = new ObservableCollection<string>(result.Select(w => w.Code).ToList());
      }
    }

    public void RaiseOnPropertyChange([CallerMemberName] string propertyName = null)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    private void validateWarehouses()
    {
      bool isValid = true;

      if (this.WarehouseOriginSelect != null)
      {
        if (this.WarehouseDestinySelect != null)
        {
          if (this.WarehouseOriginSelect.Equals(this.WarehouseDestinySelect))
          {
            isValid = false;
            App.Current.MainPage.DisplayAlert("TSHIRT", "Debe seleccionar almacenes distintos", "OK");
          }
        }
        else
        {
          isValid = false;
          App.Current.MainPage.DisplayAlert("TSHIRT", "Debe seleccionar un alamcén destino", "OK");
        }
      }
      else
      {
        isValid = false;
        App.Current.MainPage.DisplayAlert("TSHIRT", "Debe seleccionar un alamcén origen", "OK");
      }

      if (isValid)
      {
        var productsTransfer = new ProductsTransfer() { BindingContext = this };
        Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(productsTransfer);
      }

    }

    private void validateProduct()
    {
      bool isValid = true;

      if (this.warehouseProduct != null)
      {
        if (string.IsNullOrEmpty(this.Quantity))
        {
          isValid = false;
          App.Current.MainPage.DisplayAlert("TSHIRT", "Debe ingresar la cantidad", "OK");
        }
        else if (long.Parse(this.Quantity) > this.warehouseProduct.Quantity)
        {
          isValid = false;
          App.Current.MainPage.DisplayAlert("TSHIRT", "Cantidad de productos no disponible", "OK");
        }
      }
      else
      {
        isValid = false;
        App.Current.MainPage.DisplayAlert("TSHIRT", "Debe realizar la búsqueda de un producto", "OK");
      }

      if (isValid)
      {
        ProductToTransfer productTransfer = new ProductToTransfer()
        {
          productCode = warehouseProduct.Product.Code,
          productDescription = warehouseProduct.Product.Description,
          quantity = long.Parse(Quantity),
          quantityAvailable = warehouseProduct.Quantity
        };

        if (ProductTransferCollection.Count > 0)
        {

          var result = ProductTransferCollection.FirstOrDefault(p => p.productCode.Equals(warehouseProduct.ProductCode));
          if (result != null)
          {
            ProductTransferCollection.Remove(result);
          }
        }

        ProductTransferCollection.Add(productTransfer);
        HeightList = (ProductTransferCollection.Count * 45) + (ProductTransferCollection.Count * 5);
        resetProduct();
        App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Entry>("EntProducto").Focus();

      }

    }

    private void resetProduct()
    {

      this.Quantity = string.Empty;
      this.ProductCode = string.Empty;
      this.ProductName = string.Empty;
      this.warehouseProduct = null;

    }

    private async void SearchWarehouseProduct()
    {
      var result = await _productTransferServices.GetWarehouseProduct(this.WarehouseOriginSelect, ProductCode);

      if (result != null)
      {
        warehouseProduct = result;
        ProductName = warehouseProduct.Product.Description;
        App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Entry>("EntQuantity").Focus();
      }
      else
      {
        warehouseProduct = null;
        ProductName = string.Empty;
        await App.Current.MainPage.DisplayAlert("TSHIRT", "Producto no disponible", "OK");
        App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Entry>("EntProducto").Focus();
      }

      ProductCode = string.Empty;
      Quantity = string.Empty;
    }

    private async void validateTransfer()
    {
      if (ProductTransferCollection.Count == 0)
      {
        await App.Current.MainPage.DisplayAlert("TSHIRT", "Debe ingresar uno o más productos a transferir", "OK");
      }
      else
      {
        var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Desea Guardar la solicitud?", "SI", "NO");
        if (answer)
        {
          int pendiente = (int)ProductTransferEstatus.Pendiente;

          ProductTransfer productsTransfer = new ProductTransfer()
          {
            warehouseOrigin = _warehouseOriginSelect,
            warehouseDestiny = _warehouseDestinySelect,
            products = ProductTransferCollection.ToList(),
            dateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
            status = pendiente.ToString(),
            observation = Observation
          };

          var result = await _productTransferServices.SaveProductTransfer(productsTransfer);
          if (result != null)
          {
            string id = string.Concat("00", result.Id.ToString());
            id =  id.Substring(id.Length - 3);
            await Application.Current.MainPage.Navigation.PushAsync(new Result(id));
          }
          else
          {
            await App.Current.MainPage.DisplayAlert("TSHIRT", "Error al registrar la solicitud", "OK");
          }
        }
      }
    }

    private async void Delete(ProductToTransfer x)
    {
      var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "El registro " + x.productCode + " sera eliminado, Desea Continuar?", "SI", "NO");
      if (answer)
      {
        ProductTransferCollection.Remove(x);
      }

    }

    private void Cancel()
    {
      resetProduct();
      ProductTransferCollection = new ObservableCollection<ProductToTransfer>();
      WarehouseOriginSelect = string.Empty;
      WarehouseDestinySelect = string.Empty;
      Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
    }

    #endregion

    #region ICOMMAND
    public ICommand Next
    {
      get { return new RelayCommand(validateWarehouses); }

    }

    public ICommand SearchCommand
    {
      get { return new RelayCommand(SearchWarehouseProduct); }
    }

    public ICommand ProductCommand
    {
      get { return new RelayCommand(validateProduct); }
    }

    public ICommand RemoveCommand
    {
      get
      {
        return new Command((e) =>
        {
          var item = (e as ProductToTransfer);
          Delete(item);
        });
      }
    }

    public ICommand EditCommand
    {
      get
      {
        return new Command((e) =>
        {
          var item = (e as ProductToTransfer);
          warehouseProduct = new WarehouseProduct()
          {
            Quantity = item.quantityAvailable,
            Product = new Product() { Code = item.productCode, Description = item.productDescription },
            ProductCode = item.productCode
          };

          Quantity = item.quantity.ToString();
          ProductName = item.productDescription;
          App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Entry>("EntQuantity").Focus();

        });
      }
    }

    public ICommand SaveCommand
    {
      get { return new RelayCommand(validateTransfer); }

    }

    public ICommand CancelCommand
    {
      get { return new RelayCommand(Cancel); }
    }

    #endregion
  }
}
