using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TShirt.InventoryApp.Services.Mobile.Models
{
  public class Output
  {
    public int Id { get; set; }
    public string Order { get; set; }
    public string Observation { get; set; }
    public string DateCreated { get; set; }
    public string Status { get; set; }
    [NotMapped]
    public string Warehouse
    {
      get
      {
        var temp = Details != null && Details.Count > 0 ? Details[0].Warehouse : "";
        return temp;
      }
      set { }
    }
    [NotMapped]
    public List<OutputDetail> Details { get; set; }
    [NotMapped]
    public string _Id
    {
      get
      {
        var temp = string.Concat("00", Id.ToString());
        return temp.Substring(temp.Length - 3);
      }
      set { }
    }
  }
}