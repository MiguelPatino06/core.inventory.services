using System.Collections.Generic;

namespace TShirt.InventoryApp.Services.Models
{
  public class TransferDetail
  {
    private string _id;
    public string Id
    {
      get { return _id.Substring(_id.Length - 3); }
      set { _id = string.Concat("00", value); }
    }
    public string WarehouseOrigin { get; set; }
    public string WarehouseDestiny { get; set; }
    public string DateCreated { get; set; }
    public string Status { get; set; }
    public string Observation { get; set; }
    public List<ProductTransferDetailExtend> products { get; set; }
  }
}
