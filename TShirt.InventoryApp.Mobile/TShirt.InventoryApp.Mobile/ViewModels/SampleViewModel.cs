using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Pages.Transaction.Sample;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;
using Xamarin.Forms;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class SampleViewModel: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private SampleServices sampleService;
        private ProductServices productServices;

        public SampleViewModel(int? id)
        {
            sampleService = new SampleServices();
            productServices = new ProductServices();
            _cont = 0;
            ExistProduct = false;
            LoadDetails(id);

        }


        #region PROPERTIES

        private string _cliente;

        public string Cliente
        {
            get { return _cliente; }
            set
            {
                _cliente = value;
                OnPropertyChanged("Cliente");
            }
        }

        private string _searchProduct;

        public string SearchProduct
        {
            get { return _searchProduct; }
            set
            {
                _searchProduct = value;
                OnPropertyChanged("SearchProduct");
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

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged("ProductName");
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

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private ObservableCollection<SampleDetailExtend> _details = new ObservableCollection<SampleDetailExtend>();
        public ObservableCollection<SampleDetailExtend> Details
        {
            get { return _details; }
            set
            {
                _details = value;
                HeightList = (_details.Count * 45) + (_details.Count * 10);
                OnPropertyChanged("Details");
            }
        }

        private int _cont;
        public int Cont
        {
            get { return _cont; }
            set
            {
                 _cont = value;
                OnPropertyChanged("Cont");
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


        private bool _existProduct;

        public bool ExistProduct
        {
            get { return _existProduct; }
            set
            {
                _existProduct = value;
                OnPropertyChanged("ExistProduct");
            }
        }


        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;               
                OnPropertyChanged("Products");
            }
        }


        private string _productSelect;

        public string ProductSelect
        {
            get { return _productSelect; }
            set
            {
                _productSelect = value;
                OnPropertyChanged("ProductSelect");
            }
        }

        private string _messageSearch;

        public string MessageSearch
        {
            get { return _messageSearch; }
            set
            {
                _messageSearch = value;
                OnPropertyChanged("MessageSearch");
            }
        }


        #endregion


        #region COMMANDS

        public ICommand SearchCommand
        {
            get { return new RelayCommand(Add); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add); }
        }

        public ICommand UpdateCommand
        {
            get { return new RelayCommand(Update); }
        }

        public ICommand SearchProductCommand
        {
            get { return new RelayCommand(Search); }
        }


        public ICommand DeleteCommand
        {
            //get { return new RelayCommand(Delete); }
            get
            {
                return new Command((e) =>
                {
                    var item = (e as SampleDetailExtend);
                    Debug.WriteLine(@"Delete " + item.ProductCode);
                    Delete(item);
                });
            }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Add); }
        }

        #endregion


        private async void Add()
        {

            if (Quantity > 0)
            {


                if (!Details.Any(a => a.ProductCode.Trim().ToLower() == SearchProduct.Trim().ToLower()))
                {
                    Details.Add(new SampleDetailExtend()
                    {
                        Id = (Cont += 1),
                        ProductCode = SelectedValueBp,
                        ProductName = SystemElementList.FirstOrDefault(a => a.Code == SelectedValueBp).Description,
                        Quantity = Quantity,
                        User = "USER01"
                    });
                    HeightList = (_details.Count * 45) + (_details.Count * 10);
                    Quantity = 0;
                    ExistProduct = false;
                    SearchProduct = string.Empty;
                    SystemElementList = null;
                    MessageSearch = string.Empty;
                }
                else
                {
                    MessageSearch = "El producto ya se encuentra en la Muestra";
                }


            }
            else
            {
                MessageSearch = "La Cantidad ingresada debe ser mayor a 0";
            }


        }


        private async void Update()
        {

            var x = new SampleDetailExtend();
            Debug.WriteLine(@"Update " + x.ProductCode);
        }

        private async void Delete(SampleDetailExtend x)
        {
            var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "El registro " + x.ProductCode + " sera eliminado, Desea Continuar?", "SI", "NO");
            if (answer)
            {              
                Debug.WriteLine(@"Delete " + x.ProductCode);

                Details.Remove(x);
                
            }

        }

        private async void Save()
        {
            var _saveSample = new Sample();
            var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Desea Guardar el registro?", "SI", "NO");
            if (answer)
            {
                _saveSample.Client = Cliente;
                _saveSample.Observation = Observation;
                _saveSample.User = "USER01";
                _saveSample.Status = "1";
                _saveSample.Details = Details.ToList();
                var result = await sampleService.Save(_saveSample);
                if (result > 0)
                {
                   await Application.Current.MainPage.Navigation.PushAsync(new Result(result));
                }
                else
                {
                   await App.Current.MainPage.DisplayAlert("TSHIRT", "Error", "OK");
                }
            }
        }

        public async void LoadDetails(int? id)
        {
            if (id != null)
            {
                var result = await sampleService.GetById((int)id);
                if (result.Details != null)
                {

                    Cliente = result.Client;
                    Observation = result.Observation;
                    Details = new ObservableCollection<SampleDetailExtend>(result.Details);
                }
                   
            }

        }


        public async void Search()
        {
            if (!string.IsNullOrEmpty(SearchProduct))
            {
                var result = await productServices.Search(SearchProduct);
                if (result != null)
                {

                    SystemElementList = result;

                    SelectedValueBp = result.FirstOrDefault().Code;
                    ExistProduct = true;
                    MessageSearch = string.Empty;
                }
                else
                {
                    MessageSearch = "Producto no Registrado";
                    SystemElementList = null;
                    ExistProduct = false;
                }
            }
            else
            {
                MessageSearch = "Debe ingresar un código o Nombre de producto";
            }



        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region PICKER


        private IEnumerable<Product> _systemElementList;

        public IEnumerable<Product> SystemElementList
        {
            get { return _systemElementList; }
            set
            {
                _systemElementList = value;
                OnPropertyChanged("SystemElementList");
            }
        }


        private string _selectedValueBp;

        public string SelectedValueBp
        {
            get { return _selectedValueBp; }
            set
            {
                 _selectedValueBp = value;
                OnPropertyChanged("SelectedValueBp");
            }
        }


        public void LoadPicker()
        {
            var listProduct = new List<Product>();

            listProduct.Add(new Product() { Id = 1, Code = "01", Description = "producto 1"});
            listProduct.Add(new Product() { Id = 2, Code = "02", Description = "producto 2" });
            listProduct.Add(new Product() { Id = 3, Code = "03", Description = "producto 3" });
            listProduct.Add(new Product() { Id = 4, Code = "04", Description = "producto 4" });

            SystemElementList = listProduct;

            SelectedValueBp = "02";

        }
        #endregion



    }
}
