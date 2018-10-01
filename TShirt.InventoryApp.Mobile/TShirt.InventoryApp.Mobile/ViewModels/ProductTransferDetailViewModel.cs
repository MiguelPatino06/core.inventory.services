using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Services;
using static TShirt.InventoryApp.Mobile.Infrastructure.EnumTShirt;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
  public class ProductTransferDetailViewModel : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    private ProductTransferServices _productTransferServices;


    public ProductTransferDetailViewModel(int id)
    {
      ProductsCollection = new ObservableCollection<ProductTransferDetailExtend>();
      _productTransferServices = new ProductTransferServices();
      loadDetail(id);
    }


    #region Properties
    private string _warehouseOrigin;
    public string WarehouseOrigin
    {
      get { return _warehouseOrigin; }
      set { _warehouseOrigin = value; RaiseOnPropertyChange(); }
    }

    private string _warehouseDestiny;
    public string WarehouseDestiny
    {
      get { return _warehouseDestiny; }
      set { _warehouseDestiny = value; RaiseOnPropertyChange(); }
    }

    private string _observation;
    public string Observation
    {
      get { return _observation; }
      set { _observation = value; RaiseOnPropertyChange(); }
    }

    private string _dateCreated;
    public string DateCreated
    {
      get { return _dateCreated; }
      set { _dateCreated = value; RaiseOnPropertyChange(); }
    }

    private string _status;
    public string Status
    {
      get { return (_status != null ? getStatusName(_status) : _status); }
      set { _status = value; RaiseOnPropertyChange(); }
    }

    private ObservableCollection<ProductTransferDetailExtend> _productsCollection;
    public ObservableCollection<ProductTransferDetailExtend> ProductsCollection
    {
      get { return _productsCollection; }
      set
      {
        _productsCollection = value;
        HeightList = (_productsCollection.Count * 45) + (_productsCollection.Count * 5);
        RaiseOnPropertyChange();
      }
    }

    private int _heightList;
    public int HeightList
    {
      get { return _heightList; }
      set { _heightList = value; RaiseOnPropertyChange(); }
    }

    #endregion


    #region Commands


    #endregion

    #region Methods

    private string getStatusName(string status)
    {

      ProductTransferEstatus value = (ProductTransferEstatus)Enum.Parse(typeof(ProductTransferEstatus), status);

      switch (value)
      {
        case ProductTransferEstatus.Aprobada:
          return "Aprobada";
        case ProductTransferEstatus.Pendiente:
          return "Pendiente";
        case ProductTransferEstatus.Rechazada:
          return "Rechazada";
        default:
          return "Desconocido";
      }

    }


    private async void loadDetail(int id) {
      
      var result = await _productTransferServices.Get(id);

      if (result != null) {
        WarehouseOrigin = result.WarehouseOrigin;
        WarehouseDestiny = result.WarehouseDestiny;
        DateCreated = result.DateCreated;
        Status = result.Status;
        ProductsCollection = new ObservableCollection<ProductTransferDetailExtend>(result.products);
        Observation = result.Observation;
      }

    }

    public void RaiseOnPropertyChange([CallerMemberName] string propertyName = null)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion
  }
}
