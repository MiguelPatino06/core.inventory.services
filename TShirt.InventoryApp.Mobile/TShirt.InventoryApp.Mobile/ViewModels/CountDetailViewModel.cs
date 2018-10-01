using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Pages.Count;


namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class CountDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CountServices countServices;

        public CountDetailViewModel(int idPlan, string product, string user, string description, string barCodeProduct, string nameProduct)
        {
            countServices = new CountServices();
            LoadDetails(idPlan, product, user);
            PlanDescription = description;
            BarCodeProduct = barCodeProduct;
            NameProduct = nameProduct;
            User = user;
            PlanId = idPlan;
            ProductCode = product;
        }

        private async void LoadDetails(int id, string plan, string user)
        {
      
            var result = await countServices.GetByPlanAndProduct(id, plan);
            if (result.Count > 0)
            {
                var data = new List<CountPlanDetailItem>();
                data = result.Where(a => a.UserCode == user).ToList();
                Details = new ObservableCollection<CountPlanDetailItem>(result.Where(a => a.UserCode == user).ToList());
                LastRowCount = Details.LastOrDefault().Count;
                //var groupedData =
                //     data.OrderBy(p => p.Id)
                //         .GroupBy(p => p.Count.ToString())
                //         .Select(p => new ObservableGroupCollection<string, CountPlanDetailItem>(p))
                //         .ToList();


                //var groupedData =
                //     data.OrderBy(p => p.Id)
                //         .GroupBy(p => p.Count.ToString())
                //         .Select(p => new ObservableCollection<< IGrouping < string, CountPlanDetailItem >> (p))
                //         .ToList();



                //var grouped = from Model in data
                //              group Model by Model.Count into Group
                //              select new ObservableCollection<IGrouping<string, CountPlanDetailItem>>(Group.Key);


                //GroupedList = new ObservableCollection<IGrouping<string, CountPlanDetailItem>>(groupedData);
                //GroupedList = new ObservableCollection<ObservableGroupCollection<string, CountPlanDetailItem>>(groupedData);
                //ObservableGroupCollection =IGrouping<string, CountPlanDetailItem>
            }
            else
            {
                Details = new ObservableCollection<CountPlanDetailItem>(result);
                LastRowCount = 0;
            }
        }

        private async void Save()
        {
            var rct = new RctExtendModel();
          
            var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Esta seguro que desea Guardar?", "SI", "NO");
            if (answer)
            {
                var result = await countServices.SaveDetail(NewDetails.ToList());
                if (result == "OK")
                {
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Plan(PlanId));
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("TSHIRT - Error", result, "OK");
                }                  
            }
        }


        #region PROPERTIES
        private ObservableCollection<IGrouping<String, CountPlanDetailItem>> _groupedList = null;

        public ObservableCollection<IGrouping<String, CountPlanDetailItem>> GroupedList
        {
            get
            {
                return _groupedList;
            }
            set
            {
                _groupedList = value;
                HeightList = (_details.Count * 45) + (_details.Count * 10);
                OnPropertyChanged("GroupedList");
            }
        }

        // private ObservableCollection<<IGrouping<string, CountPlanDetailItem>> _groupedList = null;

        //public ObservableGroupCollection<string, CountPlanDetailItem> GroupedList
        //{
        //    get
        //    {
        //        return _groupedList;
        //    }
        //    set
        //    {
        //        _groupedList = value;
        //        OnPropertyChanged("GroupedList");
        //    }
        //}

        private ObservableCollection<CountPlanDetailItem> _details = new ObservableCollection<CountPlanDetailItem>();

        public ObservableCollection<CountPlanDetailItem> Details
        {
            get { return _details; }
            set
            {
                _details = value;
                HeightList = (_details.Count * 45) + (_details.Count * 10);
                OnPropertyChanged("Details");
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

        private string _planDescription;
        public string PlanDescription
        {
            get { return _planDescription; }
            set
            {
                _planDescription = value;
                OnPropertyChanged("PlanDescription");
            }
        }

        private string _barCodeProduct;
        public string BarCodeProduct
        {
            get { return _barCodeProduct; }
            set
            {
                _barCodeProduct = value;
                OnPropertyChanged("BarCodeProduct");
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

        private int _addQuantity;
        public int AddQuantity
        {
            get { return _addQuantity;  }
            set
            {
                _addQuantity = value;
                OnPropertyChanged("AddQuantity");
            }
        }

        private string _user;
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private ObservableCollection<CountPlanDetailItem> _newdetails = new ObservableCollection<CountPlanDetailItem>();

        public ObservableCollection<CountPlanDetailItem> NewDetails
        {
            get { return _newdetails; }
            set
            {
                _newdetails = value;
                OnPropertyChanged("NewDetails");
            }
        }

        private int _planId;
        public int PlanId
        {
            get { return _planId; }
            set
            {
                _planId = value;
                OnPropertyChanged("PlanId");
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

        private int _lastRowCount;
        public int LastRowCount
        {
            get { return _lastRowCount; }
            set
            {
                _lastRowCount = value;
                OnPropertyChanged("LastRowCount");
            }
        }

        private async void AddItem()
        {
            var details = new CountPlanDetailItem();


            details.CountPlanId = PlanId;
            details.UserCode = User;
            details.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
            details.Quantity = AddQuantity;
            details.ProductCode = ProductCode;
            details.Count = LastRowCount + 1;

            NewDetails.Add(details);

            int countItemsDetail = _details.Count == 0 ? 1 : _details.Count;

            HeightList = (countItemsDetail * 45) + (countItemsDetail * 10);

            Details.Add(details);
            AddQuantity = 0;
        }

        #endregion


        #region COMMANDS

        public ICommand QuantityByNumber
        {
            get { return new RelayCommand(AddItem); }
        }

        public ICommand SaveCount
        {
            get { return new RelayCommand(Save); }
        } 

        #endregion


        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



    }

    public class ObservableGroupCollection<S, T> : ObservableCollection<T>
    {
        private readonly S _key;

        public ObservableGroupCollection(IGrouping<S, T> group)
            : base(group)
        {
            _key = group.Key;
        }

        public S Key
        {
            get { return _key; }
        }
    }

}
