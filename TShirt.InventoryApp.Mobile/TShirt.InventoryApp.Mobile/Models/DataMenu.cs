using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Mobile.Models.Menu;

namespace TShirt.InventoryApp.Mobile.Models
{
    public static class DataMenu
    {
        public static IList<Item> DataItems { get; private set; }

        private static readonly Category C1 = new Category { Id = 100, CategoryTitle = "Transferencia entre Bodegas", Icon = "Images/Transaction.png" };
        private static readonly Category C2 = new Category { Id = 101, CategoryTitle = "Salida a Producción", Icon = "Images/Transaction.png" };
        private static readonly Category C3 = new Category { Id = 102, CategoryTitle = "Cambio de Producto", Icon = "Images/Cambio.png" };
        private static readonly Category C4 = new Category { Id = 103, CategoryTitle = "Muestra", Icon = "Images/Sample.png" };

        static DataMenu()
        {
            DataItems = new ObservableCollection<Item>()
            {
                new Item
                {
                    Id = 1,
                    ItemTitle = "Crear Solicitud",
                    Category = C3,
                    Icon = "Images/Transaction.png"
                },
                new Item
                {
                    Id = 2,
                    ItemTitle = "Ver Solicitud",
                    Category = C3,
                    Icon = "Images/Transaction.png"
                },
                new Item
                {
                    Id = 3,
                    ItemTitle = "Crear Solicitud",
                    Category = C4,
                    Icon = "Images/Transaction.png"
                },
                new Item
                {
                    Id = 4,
                    ItemTitle = "Ver Solicitud",
                    Category = C4,
                    Icon = "Images/Transaction.png"
                }
            };
        }
    }
}
