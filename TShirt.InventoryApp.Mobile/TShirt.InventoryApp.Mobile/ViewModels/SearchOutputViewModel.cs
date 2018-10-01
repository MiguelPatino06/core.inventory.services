using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TShirt.InventoryApp.Mobile.Infrastructure;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Services;
using Xamarin.Forms;


namespace TShirt.InventoryApp.Mobile.ViewModels
{
  public class SearchOutputViewModel : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    private OutputServices _outputServices;

    public SearchOutputViewModel()
    {
      _outputServices = new OutputServices();
      OutputCollection = new ObservableCollection<Output>();
      Search();
    }

    #region Properties
    private string _outputCode;
    public string OutputCode
    {
      get { return _outputCode; }
      set { _outputCode = value; RaiseOnPropertyChange(); }
    }

    private ObservableCollection<Output> _outputCollection;
    public ObservableCollection<Output> OutputCollection
    {
      get { return _outputCollection; }
      set
      {
        _outputCollection = value;
        HeightList = (_outputCollection.Count * 80) + (_outputCollection.Count * 10);
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
    
    public ICommand SearchCommand
    {
      get { return new RelayCommand(Search); }

    }

    public ICommand DetailCommand
    {
      get
      {
        return new Command((e) =>
        {
          var item = (e as Output);
          Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.Output.Detail(item.Id));
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
      int? code = null;

      if (!string.IsNullOrEmpty(OutputCode)) {
        code = int.Parse(OutputCode);
      }
        
      var result = await _outputServices.GetList(10, code);

      if (result != null)
      {
        OutputCollection = new ObservableCollection<Output>(result);
      }
      else
      {
        OutputCollection = new ObservableCollection<Output>();
      }
        
      
      HeightList = (OutputCollection.Count * 80) + (OutputCollection.Count * 10);
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
