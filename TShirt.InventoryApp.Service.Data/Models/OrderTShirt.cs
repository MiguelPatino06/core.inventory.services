using System;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Service.Data.Models
{
  [Table("OrderTShirt")]
  public class OrderTShirt
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string ProviderCode { get; set; }
    public string Value1 { get; set; }
    public string Value2 { get; set; }
    public string Value3 { get; set; }
    public string Value4 { get; set; }
    public string Value5 { get; set; }
    [SQLite.Net.Attributes.Ignore]
    public Nullable<bool> IsSelected { get; set; }
  }
}