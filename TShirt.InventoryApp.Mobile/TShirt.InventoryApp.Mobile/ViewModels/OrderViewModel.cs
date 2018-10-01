using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Infrastructure;
using TShirt.InventoryApp.Mobile.Pages.Recepcion;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;


namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private const int Open = (int)EnumTShirt.OrderStatus.OPEN;
        private const int Close = (int)EnumTShirt.OrderStatus.CLOSED;

        public event PropertyChangedEventHandler PropertyChanged;

        private OrderService orderService;
        private RctServices rctServices;


        //public OrderViewModel(string code)
        public OrderViewModel(List<OrderTShirt> codes, List<ViewOrder> order)
        {
            orderService = new OrderService();
            rctServices = new RctServices();
            LoadOrders(codes, order);
            LoadWarehouse();
        }

        #region Properties


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


        private string _lote;

        public string Lote
        {
            get { return _lote; }
            set
            {
                _lote = value;
                OnPropertyChanged("Lote");
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

        private ObservableCollection<ViewOrder> _Orders = new ObservableCollection<ViewOrder>();

        public ObservableCollection<ViewOrder> Orders
        {
            get { return _Orders; }
            set
            {
                _Orders = value;
                HeightList = (_Orders.Count * 45) + (_Orders.Count * 10);
                OnPropertyChanged("Orders");
            }
        }


        private string _message;

        public string MessageResult
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("MessageResult");
            }
        }

        private ObservableCollection<string> _warehouseCollection;
        public ObservableCollection<string> WarehouseCollection
        {
            get
            {
                return _warehouseCollection;
            }
            set
            {
                _warehouseCollection = value;
                OnPropertyChanged("WarehouseCollection");
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



        #endregion


        #region Commands

        public ICommand SaveRecepcion
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand SearchProvider
        {
          get { return new RelayCommand(Search);}    
        }

        #endregion

        #region Methods

        private async void Save()
        {
            var rct = new RctExtendModel();

            if (ValidateOrder())
            {
                var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Esta seguro que desea Guardar?", "SI", "NO");
                if (answer)
                {
                    //var _details = OrderProduct.Select(items => new Detail()
                    //{
                    //    OrderCode = items                        
                    //}).Distinct().ToList();
                 
                    var _details = Orders.Select(items => new Detail()
                    {
                        OrderCode = items.Code,
                        Status = (items.Quantity > items.TotalProduct) ? Open.ToString() : Close.ToString(),
                        ProductCode = items.ProductCode,
                        Warehouse = items.OrderValue1
                    }).ToList();



                    rct.Lot = Lote;
                    rct.ProviderCode = ProviderCode;
                    rct.UserId = 1;
                    rct.Details = _details;

                    var result = await rctServices.Add(rct);
                    if (result != null)
                        await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Rct(ProviderName, result));
                }
            }


        }

        private async void LoadOrders(List<OrderTShirt> codes, List<ViewOrder> order)
        {

            OrderProduct = codes.Select(a => a.Code).Distinct().ToArray();
            PurchaseOrder = string.Join(",", OrderProduct);
            OrdercodesCollection = new ObservableCollection<string>(OrderProduct);
            OrderSelect = OrdercodesCollection.FirstOrDefault().ToString();
            var result = order ==  null ? await orderService.GetListOrder(codes) : order;
             
           
            if (result != null)
            {
                ProviderName  = result.FirstOrDefault().ProviderName;
                ProviderCode = result.FirstOrDefault().ProviderCode;
                var list = result.Select(item => new ViewOrder()
                {
                    Id = item.Id,
                    IdOrder = item.IdOrder,
                    Code = item.Code,
                    Description = item.Description,
                    ProviderCode = item.ProviderCode,
                    ProviderName = item.ProviderName,
                    ProviderBarcode = item.ProviderBarcode,
                    ProductCode = item.ProductCode,
                    IdProduct = item.IdProduct,
                    ProductName = item.ProductName,
                    BarcodeProduct = item.BarcodeProduct,
                    Quantity = item.Quantity,
                    OrderValue1 = item.Value2,
                    OrderValue2 = item.TotalProduct >= item.Quantity ? "Images/yes.png" : "Images/no.png",
                    OrderValue3 = item.ProductCode + " " + item.ProductName,
                    OrderValue4 = item.OrderValue4,
                    OrderValue5 = item.OrderValue5,
                    TotalProduct = item.TotalProduct
                }).ToList();
                Orders = new ObservableCollection<ViewOrder>(list);
            }
        }

        private async void LoadWarehouse()
        {
            string[] list = { "T-SHIRTS", "FLEXO", "COSTURA" };

            //list
            //list.Add(new Warehouse() {Id =0, Name = "T-SHIRTS"});
            //list.Add(new Warehouse() { Id = 1, Name = "FLEXO" });
            //list.Add(new Warehouse() { Id = 2, Name = "COSTURA" });

            WarehouseCollection = new ObservableCollection<string>(list);

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private async void Search()
        {
            var details = new OrderDetailExtend();
            var _list = Orders;
            var result = _list.FirstOrDefault(a => a.BarcodeProduct == BCode);
            if (result != null)
            {

                details.order = OrderSelect; // PurchaseOrder;
                details.providerName = ProviderName;
                details.productBarcode = BCode;
                details.productName = result.ProductName; ;
                details.productCode = result.ProductCode;
                details.providerCode = result.ProviderCode;
                details.codeOrders = OrderProduct;
                details.OrderProducts = _list.Where(a => a.BarcodeProduct == BCode).Select(a => a.Code).Distinct().ToArray();
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ProductDetail(details));
            }
            else
            {
                MessageResult = "Producto no Registrado";
            }

        }




        public bool ValidateOrder()
        {
            bool result = true;

            if (string.IsNullOrEmpty(Lote))
            {
                App.Current.MainPage.DisplayAlert("TSHIRT", "Debe ingresar un codigo de Lote", "OK");
                return false;
            }

            if (Orders.Any(item => string.IsNullOrEmpty(item.OrderValue1)))
            {
                App.Current.MainPage.DisplayAlert("TSHIRT", "Ingresar nombre de Almacen en productos", "OK");
                return false;
            }
                           
            return result;

        }
        #endregion
    }
}
