using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Pages.Transaction.ProductChange;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;
using Xamarin.Forms;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class OrderReqDetailViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private ProductChangeServices services;

        public OrderReqDetailViewModel(string nroPedido)
        {
            services = new ProductChangeServices();
            LoadDetails(nroPedido);
        }

        public async void LoadDetails(string pedido)
        {
            var result = await services.GetDetailByCode(pedido);
            if (result != null)
            {
                Details = new ObservableCollection<OrderReqDetailExtend>(result.Detail);
                Pedido = pedido;
                ClientName = result.ClientName;
                Observation = result.Observation;
            }
        }


        private int _heightList;

        public int HeightList
        {
            get { return _heightList; }
            set
            {
                _heightList = value;
                OnPropertyChanged("HeightList");
            }
        }

        private string _observation;

        public string Observation
        {
            get { return _observation; }
            set
            {
                _observation = value;
                OnPropertyChanged("Observation");
            }
        }

        private string _pedido;

        public string Pedido
        {
            get { return _pedido; }
            set
            {
                _pedido = value;
                OnPropertyChanged("Pedido");
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


        private ObservableCollection<OrderReqDetailExtend> _details = new ObservableCollection<OrderReqDetailExtend>();

        public ObservableCollection<OrderReqDetailExtend> Details
        {
            get { return _details; }
            set
            {
                _details = value;
                HeightList = (_details.Count * 45) + (_details.Count * 5);
                OnPropertyChanged("Details");
            }
        }

      

        private OrderReqDetailExtend _selectedItemDetail = new OrderReqDetailExtend();

        public OrderReqDetailExtend SelectedItemDetail
        {
            get { return _selectedItemDetail; }
            set
            {
                _selectedItemDetail = value;
                AddProduct();
                OnPropertyChanged("OrderReqDetailExtend");
            }
        }



        public ICommand PressSave
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand PressCancel
        {
            get { return new RelayCommand(Cancel); }
        }



        //public ICommand RemoveCommand
        //{
        //    get { return new RelayCommand(Remove); }
        //}
        public ICommand RemoveCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var item = (e as OrderReqDetailExtend);
                    Debug.WriteLine(@"Error " + item.ProductCode);
                });
            }
        }
        private async void Remove()
        {
            var x = SelectedItemDetail;

        }

        private async void Save()
        {
            bool result = false;
            var items = new OrderReqExtend();
            items.Code = Pedido;
            items.Status = "1";
            items.Observation = Observation;


            var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Desea Guardar Cambio de Productos?", "SI", "NO");
            if (answer)
            {
                result = await services.UpdateOrder(items);
                if (result)
                {
                    await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Result());
                }
                else
                    App.Current.MainPage.DisplayAlert("TSHIRT", "Error guardando registro", "OK");

            }

        }


        private async void Cancel()
        {

            var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Presiono la opcion Cancelar, Desea Continuar?", "SI", "NO");
            if (answer)
            {
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ClientProduct());
            }

        }

        private async void AddProduct()
        {
           
            if (SelectedItemDetail.ProductCode != null)          
            {
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Add(Pedido, ClientName, SelectedItemDetail.ProductCode, SelectedItemDetail.ProductName, SelectedItemDetail.Quantity));
            }

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
