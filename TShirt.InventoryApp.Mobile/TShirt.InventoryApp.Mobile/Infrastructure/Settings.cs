using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TShirt.InventoryApp.Mobile.Infrastructure
{
    public class Settings
    {
        public readonly static Settings Default = new Settings();

        public string Provider { get; set; }
    }
}
