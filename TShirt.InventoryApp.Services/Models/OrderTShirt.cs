using System.Collections.Generic;

namespace TShirt.InventoryApp.Services.Models
{
  public class OrderTShirt
  {
    public int Id { get; set; }


    public string Code { get; set; }


    public string Description { get; set; }


    public string ProviderCode { get; set; }


    public string Value1 { get; set; } //THIS VALUE IS USED TO STATUS

    public string Value2 { get; set; }

    public string Value3 { get; set; }

    public string Value4 { get; set; }

    public string Value5 { get; set; }

    public bool IsSelected { get; set; }

    public List<OrderDetail> Details { get; set; }
  }
}
