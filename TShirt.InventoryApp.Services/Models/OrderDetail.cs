
namespace TShirt.InventoryApp.Services.Models
{
  public class OrderDetail
  {
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public string ProductCode { get; set; }
    public int Quantity { get; set; }
    public string Value1 { get; set; }
    public string Value2 { get; set; }
    public string Value3 { get; set; }
    public string Value4 { get; set; }
    public string Value5 { get; set; }
    public string DateCreated { get; set; }
  }
}
