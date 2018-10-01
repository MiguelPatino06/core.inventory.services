using Newtonsoft.Json;

namespace TShirt.InventoryApp.Services.Models
{
  public class OrderProvider
  {

    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string ProviderBarCode { get; set; }

    [JsonProperty("OrderCode")]
    public string Order { get; set; }

    public string OrderDescription { get; set; }

    [JsonProperty("ProductCode")]
    public string Product { get; set; }

    public int Quantity { get; set; }

    public string ProductBarCode { get; set; }

    public string ProductDescription { get; set; }
  }
}
