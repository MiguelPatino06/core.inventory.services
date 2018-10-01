using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TShirt.InventoryApp.Services.Models
{
  public class Sample
  {
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo es Requerido")]
    public string Client { get; set; }
    public string Observation { get; set; }
    public string User { get; set; }
    public string DateCreated { get; set; }
    public string Status { get; set; }
    public string Value1 { get; set; }
    public string Value2 { get; set; }
    public string Value3 { get; set; }
    public string Value4 { get; set; }
    public string Value5 { get; set; }
    public List<SampleDetailExtend> Details { get; set; }
  }
}
