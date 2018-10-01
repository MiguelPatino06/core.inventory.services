using System.Collections.Generic;

namespace TShirt.InventoryApp.Services.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string ProcessType { get; set; }
        public string Lot { get; set; }
        public string Code { get; set; }
        public string DateCreated { get; set; }
        public string WarehouseO { get; set; }
        public string WarehouseD { get; set; }
        public string User { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string Value6 { get; set; }
        public string Value7 { get; set; }
        public string Value8 { get; set; }
        public List<DocumentDetail> Details { get; set; }
    }
}
