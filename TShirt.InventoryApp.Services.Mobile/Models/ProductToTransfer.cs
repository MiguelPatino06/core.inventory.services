
namespace TShirt.InventoryApp.Services.Mobile.Models
{
  public class ProductToTransfer
  {
    public long quantity { get; set; }
    public long quantityAvailable { get; set; }
    public string productCode { get; set; }
    public string productDescription { get; set; }
  }
}
