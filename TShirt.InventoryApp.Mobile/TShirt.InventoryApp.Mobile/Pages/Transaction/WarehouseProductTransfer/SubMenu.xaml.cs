
using System.Collections.ObjectModel;
using System.ComponentModel;
using TShirt.InventoryApp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages.Transaction.WarehouseProductTransfer
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubMenu : ContentPage
    {
        public SubMenu()
        {
            InitializeComponent();
            BindingContext = new SubMenuViewModelTransaction();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        => ((ListView)sender).SelectedItem = null;
    }

    public class SubMenuViewModelTransaction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public SubMenuViewModelTransaction()
        {
            LoadMenu();
        }

        public void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

            Menu.Add(new MenuItemViewModel
            {
                Id = 12,
                Icon = "Images/add.png",
                Name = "Crear Solicitud",
                Page = ""
            });
            Menu.Add(new MenuItemViewModel
            {
                Id = 13,
                Icon = "Images/list.png",
                Name = "Ver Solicitud",
                Page = ""
            });

        }


    }

}
