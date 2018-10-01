using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TShirt.InventoryApp.Integration
{
   public  interface IObserver
    {
        void Publish(string msg);
    }
}
