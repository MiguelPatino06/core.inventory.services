using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.Inventory.Mobile.Pages;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Services;
using Xamarin.Forms;




namespace TShirt.Inventory.Mobile.ViewModels
{
    public class CountViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private CountServices countServices;
        private SyncServices syncServices;

        public CountViewModel(int? id)
        {
            id = 3;
            countServices = new CountServices();
            syncServices = new SyncServices();

            if (id == null)
                LoadItems();
            else
                LoadDetailsPlan((int)id);

            User = "USER01";


        }


        #region METHODS
        private async void LoadDetailsPlan(int id)
        {
            var list = new List<ViewCountPlanDetailExtend>();
            var result = await countServices.GetById(id);
            if (result != null)
            {
                list.AddRange(result.Select(item => new ViewCountPlanDetailExtend()
                {
                    Id = item.Id,
                    IdCountPlan = item.IdCountPlan,
                    Name = item.Name,
                    Description = item.Description,
                    ProductCode = item.ProductCode,
                    Quantity = item.Quantity,
                    TotalCounted = item.TotalCounted,
                    BarCode = item.BarCode,
                    ProductDescription = item.ProductDescription,
                    TotalProduct = item.TotalProduct,
                    HasDetails = (item.TotalProduct > 0) ? "Images/check.png" : "Images/uncheck.png",
                    ProductOk = (item.Quantity <= item.TotalProduct) ? "Images/yes.png" : "Images/no.png"
                }));

                if (list.Count() > 0)
                {
                    PlanName = result.FirstOrDefault().Name;
                    PlanDescription = result.FirstOrDefault().Name + " " + result.FirstOrDefault().Description;
                }


                Details = new ObservableCollection<ViewCountPlanDetailExtend>(list);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private async void LoadItems()
        {

            var result = await countServices.GetAll();
            if (result != null)
            {
                _items = new ObservableCollection<CountPlan>(result);
            }
        }


        private async void Search()
        {

            //var _list = Details;
            //var result = _list.FirstOrDefault(a => a.BarCode == BCode);
            //if (result != null)
            //{
            //    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlanDetails(result.IdCountPlan, result.ProductCode, User, PlanDescription, BCode, result.ProductDescription));
            //}
            //else
            //    MessageResult = "Producto no Registrado";


        }

        public async void Reload()
        {

            App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Button>("btnRefresh").Text = "Buscando...";
            App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Button>("btnRefresh").IsEnabled = false;

            var result = await syncServices.Execute("IV10300");

            if (result)
            {
                LoadItems();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("TSHIRT", "Error al sincronizar los planes de conteo", "OK");
            }

            App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Button>("btnRefresh").Text = "Refrescar";
            App.Current.MainPage.Navigation.NavigationStack.Last().FindByName<Button>("btnRefresh").IsEnabled = true;
        }

        public async void ValidatePlan()
        {
            bool result = true;
            foreach (var item in Details.ToList())
            {
                if (item.TotalProduct == 0) result = false;
            }

            //if (!result)
            //{
            //    var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", PlanName + " Aun tiene productos sin contar, Desea Continuar?", "SI", "NO");
            //    if (answer)
            //    {
            //        Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new CountResume(Details.ToList(), PlanName));
            //    }
            //}
            //else
            //    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new CountResume(Details.ToList(), PlanName));



        }

        public async void Close()
        {
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
        }

        #endregion

        #region PROPERTIES

        private ObservableCollection<CountPlan> _items = new ObservableCollection<CountPlan>();

        public ObservableCollection<CountPlan> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                HeightList = (_items.Count * 45) + (_items.Count * 5);
                OnPropertyChanged("Items");
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


        private ObservableCollection<ViewCountPlanDetailExtend> _details = new ObservableCollection<ViewCountPlanDetailExtend>();

        public ObservableCollection<ViewCountPlanDetailExtend> Details
        {
            get { return _details; }
            set
            {
                _details = value;
                HeightList = (_details.Count * 45) + (_details.Count * 5);
                OnPropertyChanged("Details");
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


        private string _messageResult;

        public string MessageResult
        {
            get { return _messageResult; }
            set
            {
                _messageResult = value;
                OnPropertyChanged("MessageResult");
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

        private string _plan;

        public string PlanName
        {
            get { return _plan; }
            set
            {
                _plan = value;
                OnPropertyChanged("PlanName");
            }
        }
        #endregion

        #region COMMANDS
        public ICommand ProcessPlan
        {
            get { return new RelayCommand(ValidatePlan); }
        }

        public ICommand ClosePlan
        {
            get { return new RelayCommand(Close); }
        }

        public ICommand SearchProduct
        {
            get { return new RelayCommand(Search); }
        }

        public ICommand Refresh
        {
            get { return new RelayCommand(Reload); }
        }
        #endregion


    }
}
