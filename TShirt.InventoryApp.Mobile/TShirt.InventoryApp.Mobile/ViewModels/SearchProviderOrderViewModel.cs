using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.Pages.Recepcion;
using TShirt.InventoryApp.Services.Mobile.Services;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class SearchProviderOrderViewModel : INotifyPropertyChanged
    {

        private const string PROVEEDOR = "PR";
        private const string ORDENCOMPRA = "OC";
        public event PropertyChangedEventHandler PropertyChanged;

        private ProviderService providerService;
        private OrderService orderService;

        private string _searchName;
        private string _pressButton;


        public SearchProviderOrderViewModel()
        {
            providerService = new ProviderService();
            orderService = new OrderService();
            Providers = new ObservableCollection<Provider>();
            PressButton = PROVEEDOR;
        }


        #region Properties


        public ObservableCollection<Provider> Providers { get; set; }

        public string SearchName
        {
            get { return _searchName; }
            set { _searchName = value; RaiseOnPropertyChange(); }
        }

        public string PressButton
        {
            get { return _pressButton; }
            set { _pressButton = value; RaiseOnPropertyChange(); }
        }

        private bool _hasCoincidence;

        public bool HasCoincidence
        {
            get { return _hasCoincidence; }
            set { _hasCoincidence = value; RaiseOnPropertyChange(); }
        }

        private string _messageResult;
        public string MessageResult
        {
            get { return _messageResult; }
            set { _messageResult = value; RaiseOnPropertyChange(); }
        }

        #endregion


        #region Commands

        public ICommand SearchCommand
        {
            get { return new RelayCommand(Start); }
        }

        public ICommand PressProv
        {
            get { return new RelayCommand(SelectOptionPro); }

        }

        public ICommand PressOC
        {
            get { return new RelayCommand(SelectOptionOC); }

        }
        public string Code { get; set; }


        #endregion

        #region Methods

        public void RaiseOnPropertyChange([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SelectOptionPro()
        {
            PressButton = PROVEEDOR;
        }

        private void SelectOptionOC()
        {
            PressButton = ORDENCOMPRA;
        }

        private async void Start()
        {
            var x = PressButton;
            MessageResult = "";
            if (PressButton == PROVEEDOR)
            {
                var list = await providerService.GetProviderName(SearchName);
                Providers.Clear();

                HasCoincidence = (list.Count > 0) ? true : false;
                foreach (var item in list)
                {
                    Providers.Add(new Provider()
                    {
                        Code = item.Code,
                        Name = item.Name
                    });
                }
            }
            else
            {
                var _list = new List<OrderTShirt>();
                _list.Add(new OrderTShirt() { Code = SearchName });
                SearchOrder(_list);
            }

        }

    private async void SearchOrder(List<OrderTShirt> codes)
    {
      var result = await orderService.GetListOrder(codes);
      if (result != null)
      {

        if (result.Any(a => a.Value1.Trim() == "0"))
          await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new OrdersPage(codes, result));
        else
          MessageResult = "Order de Compra CERRADA";
      }
      else
        MessageResult = "Order de Compra NO registrada ";
    }


    #endregion
  }
}
