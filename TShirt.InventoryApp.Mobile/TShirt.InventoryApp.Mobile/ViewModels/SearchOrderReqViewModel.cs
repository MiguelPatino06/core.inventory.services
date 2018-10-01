using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class SearchOrderReqViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        
        private ProductChangeServices services;

        public SearchOrderReqViewModel()
        {
            services = new ProductChangeServices();

        }


        #region PROPERTIES
        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged("Visible");
            }
        }

        private string _nroPedido;

        public string NroPedido
        {
            get { return _nroPedido; }
            set
            {
                _nroPedido = value;
                OnPropertyChanged("NroPedido");
            }
        }


        private string _clientName;

        public string ClientName
        {
            get { return _clientName; }
            set
            {
                _clientName = value;
                OnPropertyChanged("ClientName");
            }
        }

        #endregion

        public ICommand SearchClicImage
        {
            get { return new RelayCommand(SearchImage); }
        }

        public ICommand Next
        {
            get { return new RelayCommand(NextPage); }
        }



        private async void SearchImage()
        {
            var result = new OrderReqExtend();
            result = await services.GetOrderByCode(NroPedido);
            if (result.Id >0 )
            {
                Visible = true;
                ClientName = result.ClientName;
            }               
            else
            {
                Visible = false;
                ClientName = "Pedido NO encontrado";
            }
            OnPropertyChanged("SearchImage");


        }

        private async void NextPage()
        {
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Pages.Transaction.ProductChange.Detail(NroPedido));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


     
    }
}
