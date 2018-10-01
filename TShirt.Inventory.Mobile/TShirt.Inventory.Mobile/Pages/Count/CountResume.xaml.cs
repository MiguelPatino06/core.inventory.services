using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TShirt.Inventory.Mobile.ViewModels;
using TShirt.InventoryApp.Services.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.Inventory.Mobile.Pages.Count
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountResume : ContentPage
    {
        public CountResume(List<ViewCountPlanDetailExtend> plan, string planName)
        {
            InitializeComponent();
            BindingContext = new CountResumeViewModel(plan, planName);
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Menu());
        }
    }

}
