using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.Pages.Transaction;
using TShirt.InventoryApp.Mobile.Pages.Transaction.Output;
using TShirt.InventoryApp.Services.Mobile.Services;
using Xamarin.Forms;
using static TShirt.InventoryApp.Mobile.Infrastructure.EnumTShirt;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
  public class OutputProductsViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private OutputServices _outputServices;
    private WarehouseServices _warehouseServices;

    public OutputProductsViewModel()
    {
      _warehouseServices = new WarehouseServices();
      _outputServices = new OutputServices();
      OutputProductCollection = new ObservableCollection<OutputDetail>();
      LoadWarehouses();
    }

    #region Properties

    private ObservableCollection<string> _warehouseCollection;
    public ObservableCollection<string> WarehouseCollection
    {
      get { return _warehouseCollection; }
      set { _warehouseCollection = value; RaiseOnPropertyChange(); }
    }

    private ObservableCollection<OutputDetail> _outputProductCollection;
    public ObservableCollection<OutputDetail> OutputProductCollection
    {
      get { return _outputProductCollection; }
      set
      {
        _outputProductCollection = value;
        HeightList = (_outputProductCollection.Count * 45) + (_outputProductCollection.Count * 5);
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

    private string _order;
    public string Order
    {
      get { return _order; }
      set { _order = value; RaiseOnPropertyChange(); }
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
        App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Entry>("EntOrder").Focus();
      }
    }

    public void RaiseOnPropertyChange([CallerMemberName] string propertyName = null)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
        OutputDetail outputDetail = new OutputDetail()
        {
          ProductCode = warehouseProduct.Product.Code,
          ProductDescription = warehouseProduct.Product.Description,
          Quantity = int.Parse(Quantity),
          QuantityAvailable = int.Parse(warehouseProduct.Quantity.ToString()),
          Warehouse = _warehouseOriginSelect
        };

        if (OutputProductCollection.Count > 0)
        {
          var result = OutputProductCollection.FirstOrDefault(p => p.ProductCode.Equals(warehouseProduct.ProductCode) && p.Warehouse.Equals(WarehouseOriginSelect));
          if (result != null)
          {
            OutputProductCollection.Remove(result);
          }
        }

        OutputProductCollection.Add(outputDetail);
        HeightList = (OutputProductCollection.Count * 45) + (OutputProductCollection.Count * 5);
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
      if (string.IsNullOrEmpty(WarehouseOriginSelect))
      {
        await App.Current.MainPage.DisplayAlert("TSHIRT", "Debe seleccionar un almacén origen", "OK");
      }
      else
      {
        var result = await _outputServices.GetWarehouseProduct(this.WarehouseOriginSelect, ProductCode);

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
      }

      ProductCode = string.Empty;
      Quantity = string.Empty;
    }

    private async void validateOutput()
    {
      if (OutputProductCollection.Count == 0)
      {
        await App.Current.MainPage.DisplayAlert("TSHIRT", "Debe ingresar uno o más productos", "OK");
      }
      else if (string.IsNullOrEmpty(WarehouseOriginSelect))
      {
        await App.Current.MainPage.DisplayAlert("TSHIRT", "Debe seleccionar un almacén origen", "OK");
      }
      else
      {
        var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Desea Guardar la solicitud?", "SI", "NO");
        if (answer)
        {
          int pendiente = (int)ProductTransferEstatus.Pendiente;

          Output output = new Output()
          {
            Order = Order,
            Details = OutputProductCollection.ToList(),
            DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
            Status = pendiente.ToString(),
            Observation = Observation
          };

          var result = await _outputServices.SaveOutput(output);
          if (result.Id > 0)
          {
            string id = string.Concat("00", result.Id.ToString());
            id = id.Substring(id.Length - 3);
            await Application.Current.MainPage.Navigation.PushAsync(new Result(id));
          }
          else
          {
            await App.Current.MainPage.DisplayAlert("TSHIRT", "Error al registrar la solicitud", "OK");
          }
        }
      }
    }

    private async void Delete(OutputDetail x)
    {
      var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "El registro " + x.ProductCode + " sera eliminado, Desea Continuar?", "SI", "NO");
      if (answer)
      {
        OutputProductCollection.Remove(x);
        App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Entry>("EntProducto").Focus();
      }
    }

    private void Cancel()
    {
      resetProduct();
      OutputProductCollection = new ObservableCollection<OutputDetail>();
      WarehouseOriginSelect = string.Empty;
      Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
    }

    #endregion

    #region ICOMMAND
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
          var item = (e as OutputDetail);
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
          var item = (e as OutputDetail);
          warehouseProduct = new WarehouseProduct()
          {
            Quantity = item.QuantityAvailable,
            Product = new Product() { Code = item.ProductCode, Description = item.ProductDescription },
            ProductCode = item.ProductCode,
            WarehouseCode = item.Warehouse 
          };

          WarehouseOriginSelect = item.Warehouse;
          Quantity = item.Quantity.ToString();
          ProductName = item.ProductDescription;
          App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Entry>("EntQuantity").Focus();

        });
      }
    }

    public ICommand SaveCommand
    {
      get { return new RelayCommand(validateOutput); }

    }

    public ICommand CancelCommand
    {
      get { return new RelayCommand(Cancel); }
    }

    #endregion
  }
}
