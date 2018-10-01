using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TShirt.InventoryApp.Services.Mobile.Models
{
  public class OutputDetail
  {
    public int Id { get; set; }
    public string ProductCode { get; set; }
    public int Quantity { get; set; }
    public int OutputId { get; set; }
    public string Warehouse { get; set; }
    [NotMapped]
    public string ProductDescription { get; set; }
    [NotMapped]
    public int QuantityAvailable { get; set; }
  }
}