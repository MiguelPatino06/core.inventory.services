using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Services;
using static TShirt.InventoryApp.Mobile.Infrastructure.EnumTShirt;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
  public class OutputDetailViewModel : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    private OutputServices _outputServices;

    public OutputDetailViewModel(int id)
    {
      ProductsCollection = new ObservableCollection<OutputDetail>();
      _outputServices = new OutputServices();
      loadDetail(id);
    }

    #region Properties
    private string _warehouseOrigin;
    public string WarehouseOrigin
    {
      get { return _warehouseOrigin; }
      set { _warehouseOrigin = value; RaiseOnPropertyChange(); }
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

    private ObservableCollection<OutputDetail> _productsCollection;
    public ObservableCollection<OutputDetail> ProductsCollection
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
      
      var result = await _outputServices.Get(id);

      if (result != null) {
        Order = result.Order;
        WarehouseOrigin = result.Warehouse;
        DateCreated = result.DateCreated;
        Status = result.Status;
        ProductsCollection = new ObservableCollection<OutputDetail>(result.Details);
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
