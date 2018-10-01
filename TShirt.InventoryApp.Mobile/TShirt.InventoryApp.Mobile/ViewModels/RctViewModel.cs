using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class RctViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private OrderService orderService;
        private RctServices rctServices;

        public RctViewModel(string providerName, RctExtendModel rct)
        {
            //Codes = "OC " + codes;
            ProviderName = providerName;
            Lot = rct.Lot;
            RctCode = "RCT" + rct.Id;
            InitializeView(rct.Details);

            int height = (40 * Orders.Count) + (5 * Orders.Count);
            HeightList = height.ToString();
        }

        #region PROPERTIES

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

        private string _rctCode;

        public string RctCode
        {
            get { return _rctCode; }
            set
            {
                _rctCode = value;
                OnPropertyChanged("RctCode");
            }
        }

        private string _lot;

        public string Lot
        {
            get { return _lot; }
            set
            {
                _lot = value;
                OnPropertyChanged("Lot");
            }
        }

        private ObservableCollection<Detail> _Orders = new ObservableCollection<Detail>();

        public ObservableCollection<Detail> Orders
        {
            get { return _Orders; }
            set
            {
                _Orders = value;
                OnPropertyChanged("Orders");
            }
        }


        private string _heightList;

        public string HeightList
        {
            get { return _heightList; }
            set
            {
                _heightList = value;
                OnPropertyChanged("HeightList");
            }
        }

        #endregion

        #region METHODS

        private void InitializeView(List<Detail> details)
        {
            Orders = new ObservableCollection<Detail>(details);
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
