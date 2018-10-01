using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Pages.Recepcion;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class OrderDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private OrderService _services;

        
        public OrderDetailViewModel(OrderDetailExtend extend)
        {
            OrderProduct = extend.codeOrders; //contains order number 
            OrderProducts = extend.OrderProducts; //contains order from picker
            _services = new OrderService();
            LoadOrders(extend.order);
            PurchaseOrder = extend.order;
            ProviderName = extend.providerName;
            BCode = extend.productBarcode;
            NameProduct = extend.productName;
            ProviderCode = extend.providerCode;
            ProductCode = extend.productCode;
            //OrderProduct = extend.codeOrders;
            
            AddUserCode = "Miguel Patino";
        }


        #region METHODS

        private async void LoadOrders(string code)
        {

            var list = new List<OrderDetailProduct>();
            foreach (var item in OrderProduct)
            {
                list.Add(new OrderDetailProduct() {OrderCode = item });
            }


            var result = await _services.GetOrderDetailProduct(list);
            if (result != null)
            {
                var filterByProduct = result.Where(a => a.ProductCode == ProductCode).ToList();
                Details = new ObservableCollection<OrderDetailProduct>(filterByProduct);
                CountProduct = int.Parse(filterByProduct.Sum(a => a.Quantity).ToString());              
            }
            OrdercodesCollection = new ObservableCollection<string>(OrderProducts);
            OrderSelect = OrdercodesCollection.FirstOrDefault().ToString();

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private async void Save()
        {
            var _items = new OrderDetailProduct();

            _items.OrderCode = OrderSelect; // PurchaseOrder;
            _items.UserCode = "Miguel Patino";
            _items.DateCreated = DateTime.Now.ToString("g");
            _items.Quantity = AddQuantity; //   int.Parse(QuantityProduct.Trim());
            _items.ProductCode = ProductCode;
            _items.ProviderCode = ProviderCode;
                                                              
            var result = await _services.AddOrdeDetailProduct(_items);
            if (result != null)
            {
                MoveOrderPage();
                //CountProduct += int.Parse(QuantityProduct.Trim());
                //QuantityProduct = "";
                //Details.Add(_items);
                //HeightList = (_details.Count * 45) + (_details.Count * 5);
            }
            //else
            //TODO message
        }

        private async void AddByBarcode()
        {


        }

        private async void AddByNumber()
        {
            //var x = AddQuantity.Trim();
            //int _number = string.IsNullOrEmpty(x) ? 0 : int.Parse(x);

            if (AddQuantity > 0)
                Save();
            else
                App.Current.MainPage.DisplayAlert("TSHIRT", "Debe ingresar un numero Mayor a 0", "OK");

        }

        private async void MoveOrderPage()
        {
            var _list = new List<OrderTShirt>();

            foreach (var item in OrderProduct)
            {
                _list.Add(new OrderTShirt() {Code = item});
            }
            
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new OrdersPage(_list, null));
        }

        #endregion


        #region PROPERTIES
        private ObservableCollection<OrderDetailProduct> _details = new ObservableCollection<OrderDetailProduct>();

        public ObservableCollection<OrderDetailProduct> Details
        {
            get { return _details; }
            set
            {
                _details = value;
                HeightList = (_details.Count * 45) + (_details.Count * 15);
                OnPropertyChanged("Details");
            }
        }

        private string _providerName;

        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                _providerName = value;
                OnPropertyChanged("ProviderName");
            }
        }

        private string _bCode;

        public string BCode
        {
            get { return _bCode; }
            set
            {
                _bCode = value;
                OnPropertyChanged("BCode");
            }
        }

        private string _nameProduct;

        public string NameProduct
        {
            get { return _nameProduct; }
            set
            {
                _nameProduct = value;
                OnPropertyChanged("NameProduct");
            }
        }

        private string _purchaseOrder;

        public string PurchaseOrder
        {
            get { return _purchaseOrder; }
            set
            {
                _purchaseOrder = value;
                OnPropertyChanged("PurchaseOrder");
            }
        }

        private string _quantityProduct;

        public string QuantityProduct
        {
            get { return _quantityProduct; }
            set
            {
                _quantityProduct = value;
                OnPropertyChanged("QuantityProduct");
            }
        }

        private string _productCode;

        public string ProductCode
        {
            get { return _productCode; }
            set
            {
                _productCode = value;
                OnPropertyChanged("ProductCode");
            }
        }

        private string _providerCode;

        public string ProviderCode
        {
            get { return _providerCode; }
            set
            {
                _providerCode = value;
                OnPropertyChanged("ProviderCode");
            }
        }

        private int _countProduct;

        public int CountProduct
        {
            get { return _countProduct; }
            set
            {
                _countProduct = value;
                OnPropertyChanged("CountProduct");
            }
        }


        private string[] _orderProduct;

        public string[] OrderProduct
        {
            get { return _orderProduct; }
            set
            {
                _orderProduct = value;
                OnPropertyChanged("OrderProduct");
            }
        }

        private string[] _orderProducts;

        public string[] OrderProducts
        {
            get { return _orderProducts; }
            set
            {
                _orderProducts = value;
                OnPropertyChanged("OrderProducts");
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

        private ObservableCollection<string> _ordercodesCollection;
        public ObservableCollection<string> OrdercodesCollection
        {
            get
            {
                return _ordercodesCollection;
            }
            set
            {
                _ordercodesCollection = value;
                OnPropertyChanged("OrdercodesCollection");
            }
        }

        private string _orderSelect;

        public string OrderSelect
        {
            get { return _orderSelect; }
            set
            {
                _orderSelect = value;
                OnPropertyChanged("OrderSelect");
            }
        }

        public int _addQuantity;

        public int AddQuantity
        {
            get { return _addQuantity; }
            set
            {
                _addQuantity = value;
                OnPropertyChanged("AddQuantity");
            }
        }

        public string _addUserCode;

        public string AddUserCode
        {
            get { return _addUserCode; }
            set
            {
                _addUserCode = value;
                OnPropertyChanged("AddUserCode");
            }
        }

        #endregion


        #region ICOMMAND

        public ICommand SaveRecepcion
        {
            //get { return new RelayCommand(MoveOrderPage); } Save
            get { return new RelayCommand(Save); }
            
        }

        public ICommand QuantityByBarcode
        {
            get { return new RelayCommand(AddByBarcode); }
        }

        public ICommand QuantityByNumber
        {
            get { return new RelayCommand(AddByNumber); }
        }

      


        #endregion



    }
}
