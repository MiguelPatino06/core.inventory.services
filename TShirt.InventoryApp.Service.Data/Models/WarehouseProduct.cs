using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Service.Data.Models
{
  [Table("WarehouseProduct")]
  public class WarehouseProduct
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string WarehouseCode { get; set; }
    public string ProductCode { get; set; }
    public long Quantity { get; set; }
  }
}