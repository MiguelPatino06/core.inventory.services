using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Infrastructure;
using TShirt.InventoryApp.Mobile.Pages.Count;
using TShirt.InventoryApp.Mobile.Properties;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;
using Xamarin.Forms;

namespace TShirt.InventoryApp.Mobile.ViewModels
{
    public class CountResumeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Color COLORDEFAULT = Color.Green;
        private CountServices countServices;

        public CountResumeViewModel(List<ViewCountPlanDetailExtend> plan, string planName)
        {
            countServices = new CountServices();
            int total =0 , _quantity=0, _totalProduct=0;
            var list = new List<ViewCountPlanDetailExtend>();


            Porcentaje = int.Parse(Resources.PorcentajeConteo); //Obtiene valor porcentaje
           
            //CALCULA PORCENTAJE
            foreach (var row in plan)
            {
                _quantity += row.Quantity;
                _totalProduct += row.TotalProduct;
            }

            total = (((_totalProduct*100)/_quantity) - 100) * -1;
            //FIN CALCULA PORCENTAJE

            //ASIGNA COLOR E IMAGEN SEGUN PORCENTAJE
            if (total == 0)
            {
                EstatusPlanConteo = (int) EnumTShirt.CountEstatus.SINDIFERENCIA;
                ImageStatusPlan = "Images/yes.png";
                ColorText = Color.Green;
            }
            else if (total < Porcentaje)
            {
                EstatusPlanConteo = (int)EnumTShirt.CountEstatus.DIFERENCIAMEDIA;
                ImageStatusPlan = "Images/warning.png";
                ColorText = Color.Navy;
            }
            else
            {
                EstatusPlanConteo = (int)EnumTShirt.CountEstatus.DIFERENCIAMAYOR;
                ImageStatusPlan = "Images/no.png";
                ColorText = Color.Red;
            }

            foreach (var item in plan)
            {
                list.Add(
                    new ViewCountPlanDetailExtend()
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
                        HasDetails = item.HasDetails,
                        ProductOk = item.ProductOk,
                        RColor = (item.Quantity <= item.TotalProduct) ? COLORDEFAULT : ColorText
                    });
            }

            Details = new ObservableCollection<ViewCountPlanDetailExtend>(list);

            PlanName = planName;

          
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

        private string _planName;
        public string PlanName
        {
            get { return _planName; }
            set
            {
                _planName = value;
                OnPropertyChanged("PlanName");
            }
        }


        private Color _colorText;
        public Color ColorText
        {
            get { return _colorText; }
            set
            {
                _colorText = value;
                OnPropertyChanged("ColorText");
            }
        }

        private string _imageStatusPlan;
        public string ImageStatusPlan
        {
            get { return _imageStatusPlan; }
            set
            {
                _imageStatusPlan = value;
                OnPropertyChanged("ImageStatusPlan");
            }
        }

        private int _porcentaje;
        public int Porcentaje
        {
            get { return _porcentaje; }
            set
            {
                _porcentaje = value;
                OnPropertyChanged("Porcentaje");
            }
        }

        private int _estatusPlanConteo;
        public int EstatusPlanConteo
        {
            get { return _estatusPlanConteo; }
            set
            {
                _estatusPlanConteo = value;
                OnPropertyChanged("EstatusPlanConteo");
            }
        }

        public ICommand SavePlan
        {
            get { return new RelayCommand(Save); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void Save()
        {
            var _plan = new CountPlan();
            bool result;
            _plan.Id = Details.Select(a => a.IdCountPlan).FirstOrDefault();


            if (EstatusPlanConteo == (int) EnumTShirt.CountEstatus.SINDIFERENCIA)
            {
                _plan.Value2 = false.ToString();
                var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Desea Guardar el Plan de Conteo", "SI", "NO");
                if (answer)
                {
                    result = await countServices.SaveCountPlan(_plan);
                    if (result)
                    {
                        Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlantList());
                    }
                }
            }
            else if (EstatusPlanConteo == (int) EnumTShirt.CountEstatus.DIFERENCIAMEDIA)
            {
                _plan.Value2 = false.ToString();
                var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Diferencia MENOR al " + Porcentaje + "%, Desea Continuar?", "SI", "NO");
                if (answer)
                {
                    result = await countServices.SaveCountPlan(_plan);
                    if (result)
                    {
                        Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlantList());
                    }
                }
            }
            else
            {
                _plan.Value2 = true.ToString();
                var answer = await App.Current.MainPage.DisplayAlert("TSHIRT", "Diferencia MAYOR al " + Porcentaje + "%, Desea Continuar?", "SI", "NO");
                if (answer)
                {
                    result = await countServices.SaveCountPlan(_plan);
                    if (result)
                    {
                        Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlantList());
                    }
                    else
                    {
                        
                    }
                }
            }                  
        }
    }
}
