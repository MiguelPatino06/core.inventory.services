using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Service.Data.Models
{
  [Table("WareHouse")]
  public class Warehouse
  {
    [PrimaryKey]
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
  }
}