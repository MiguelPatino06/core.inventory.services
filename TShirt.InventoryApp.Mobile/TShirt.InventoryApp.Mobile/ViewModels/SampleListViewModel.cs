using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class SampleListViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private SampleServices sampleService;

        public SampleListViewModel()
        {
            sampleService = new SampleServices();
            LoadList();
            
        }


        public async void LoadList()
        {
            var result = await sampleService.GetList();
            if (result != null)
            {
                SampleList = new ObservableCollection<ViewSampleSumProduct>(result.OrderByDescending(a=> a.Id).Take(10));
            }
        }


        #region PROPERTIES
        private ObservableCollection<ViewSampleSumProduct> _sampleLists = new ObservableCollection<ViewSampleSumProduct>();

        public ObservableCollection<ViewSampleSumProduct> SampleList
        {
            get { return _sampleLists; }
            set
            {
                _sampleLists = value;
                HeightList = (_sampleLists.Count * 45) + (_sampleLists.Count * 5);
                OnPropertyChanged("SampleList");
            }
        }

        private int _searchSample;

        public int SearchSample
        {
            get { return _searchSample; }
            set
            {
                _searchSample = value;
                OnPropertyChanged("SearchSample");
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

        #endregion


        public ICommand SearchCommand 
        {
            get { return new RelayCommand(Search); }
        }

        private async void Search()
        {
            if (!string.IsNullOrEmpty(SearchSample.ToString()))
            {
                SampleList.Where(a => a.Id == SearchSample);
            }
            else
            {
                MessageSearch = "Debe ingresar un Número";
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
