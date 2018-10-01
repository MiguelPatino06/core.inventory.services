using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Mobile.ViewModels;


namespace TShirt.InventoryApp.Mobile.Infrastructure
{
    public class InstanceLocator
    {
        public InstanceLocator()
        {
            Main = new MainViewModel();
        }

        public MainViewModel Main { get; set; }
    }
}
