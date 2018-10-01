using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Service.Data.Models
{
  [SQLite.Net.Attributes.Table("OrderReq")]
    public class OrderReq
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ClientCode { get; set; }
        public string DateCreated { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string Observation { get; set; }
    }
}