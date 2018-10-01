using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TShirt.InventoryApp.Mobile.Models.Menu
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryTitle { get; set; }
        public string Icon { get; set; }

    }

    public class Item
    {
        public int Id { get; set; }
        public string ItemTitle { get; set; }
        public string Icon { get; set; }

        public Category Category { get; set; }
    }
}
