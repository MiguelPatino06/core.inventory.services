using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Properties;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Config : ContentPage
    {
        public Config()
        {
            InitializeComponent();
            BindingContext = new ConfigViewModel();
        }
    }

    class ConfigViewModel : INotifyPropertyChanged
    {
        private ConfigurationServices services;

        public ConfigViewModel()
        {
            services= new ConfigurationServices();
            Text1 = Resources.Text1;
            Text2 = Resources.Text2;
            Text3 = Resources.Text3;
            Text4 = Resources.Text4;
            Text5 = Resources.Text5;
            LoadValues();
        }

        private int _id { get; set; }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }

        private string _value1 { get; set; }
        public string Value1
        {
            get { return _value1; }
            set
            {
                _value1 = value;
                OnPropertyChanged("Value1");
            }
        }

        private string _value2 { get; set; }
        public string Value2
        {
            get { return _value2; }
            set
            {
                _value2 = value;
                OnPropertyChanged("Value2");
            }
        }

        private string _value3 { get; set; }
        public string Value3
        {
            get { return _value3; }
            set
            {
                _value3 = value;
                OnPropertyChanged("Value3");
            }
        }

        private string _value4 { get; set; }
        public string Value4
        {
            get { return _value4; }
            set
            {
                _value4 = value;
                OnPropertyChanged("Value4");
            }
        }
        private string _value5 { get; set; }
        public string Value5
        {
            get { return _value5; }
            set
            {
                _value5 = value;
                OnPropertyChanged("Value5");
            }
        }


        public ICommand SaveConfig
        {
            get { return new RelayCommand(Save);}
        }



        public async void LoadValues()
        {
            var result = await services.Get();
            if (result != null)
            {
                Id = result.Id;
                Value1 = result.Value1;
                Value2 = result.Value2;
                Value3 = result.Value3;
                Value4 = result.Value4;
                Value5 = result.Value5;
            }
        }


        private async void Save()
        {
            var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Esta seguro que desea Guardar?", "SI", "NO");
            if (answer)
            {
                var configuration = new Configuration();
                configuration.Id = Id;
                configuration.Value1 = Value1;
                configuration.Value2 = Value2;
                configuration.Value3 = Value3;
                configuration.Value4 = Value4;
                configuration.Value5 = Value5;

                bool result = await services.Save(configuration);
                if (result)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Page2());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("TSHIRT", "Erro, Consulte administrador", "OK");
                }
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
