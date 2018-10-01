using System;
using System.Collections.Generic;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Count
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
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Page2());
        }
    }

   
}
