using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TShirt.InventoryApp.Mobile.Models;
using TShirt.InventoryApp.Mobile.Pages.Recepcion;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.ProductChange
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detail : ContentPage
    {
        public Detail(string nroPedido)
        {
            InitializeComponent();
            BindingContext = new OrderReqDetailViewModel(nroPedido);
        }

    }

  
}
