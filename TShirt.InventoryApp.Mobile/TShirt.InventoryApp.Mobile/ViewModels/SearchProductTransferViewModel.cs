using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TShirt.InventoryApp.Mobile.Infrastructure;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Services;
using Xamarin.Forms;


namespace TShirt.InventoryApp.Mobile.ViewModels
{
  public class SearchProductTransferViewModel : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    private ProductTransferServices _productTransferServices;


    public SearchProductTransferViewModel()
    {
      _productTransferServices = new ProductTransferServices();
      ProductTransferSummaryCollection = new ObservableCollection<TransferDetail>();
      Search();
    }


    #region Properties
    private string _transferCode;
    public string TransferCode
    {
      get { return _transferCode; }
      set { _transferCode = value; RaiseOnPropertyChange(); }
    }

    private ObservableCollection<TransferDetail> _productTransferSummaryCollection;
    public ObservableCollection<TransferDetail> ProductTransferSummaryCollection
    {
      get { return _productTransferSummaryCollection; }
      set
      {
        _productTransferSummaryCollection = value;
        HeightList = (_productTransferSummaryCollection.Count * 80) + (_productTransferSummaryCollection.Count * 10);
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
    
    public ICommand SearchRequestCommand
    {
      get { return new RelayCommand(Search); }

    }

    public ICommand DetailCommand
    {
      get
      {
        return new Command((e) =>
        {
          var item = (e as TransferDetail);
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.WarehouseProductTransfer.Detail(int.Parse(item.Id)));
        });
      }
    }
    #endregion

    #region Methods
    private string getStatusName(string status) {

      EnumTShirt.ProductTransferEstatus value = (EnumTShirt.ProductTransferEstatus)Enum.Parse(typeof(EnumTShirt.ProductTransferEstatus), status);

      switch (value)
      {
        case EnumTShirt.ProductTransferEstatus.Aprobada:
          return "Aprobada";
        case EnumTShirt.ProductTransferEstatus.Pendiente:
          return "Pendiente";
        case EnumTShirt.ProductTransferEstatus.Rechazada:
          return "Rechazada";
        default:
          return "Desconocido";
      }

    }

    public async void Search()
    {
      var result = await _productTransferServices.GetRequests(1, 10, TransferCode);

      if (result != null)
      {
        var _result = result.Item1.Select(item => new TransferDetail()
        {
          Id = item.Id,
          DateCreated = item.DateCreated,
          Observation = item.Observation,
          products = item.products,
          Status = getStatusName(item.Status),
          WarehouseDestiny = item.WarehouseDestiny,
          WarehouseOrigin = item.WarehouseOrigin
        });

        ProductTransferSummaryCollection = new ObservableCollection<TransferDetail>(_result.ToList());
      }
      else
      {
        ProductTransferSummaryCollection = new ObservableCollection<TransferDetail>();
      }
        
      
      HeightList = (ProductTransferSummaryCollection.Count * 80) + (ProductTransferSummaryCollection.Count * 10);
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
