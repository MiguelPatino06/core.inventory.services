using Xamarin.Forms;

namespace TShirt.InventoryApp.Services.Mobile.Models
{
  public class ViewCountPlanDetailExtend
  {
    public int Id { get; set; }
    public int IdCountPlan { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProductCode { get; set; }
    public int Quantity { get; set; }
    public int? TotalCounted { get; set; }
    public string BarCode { get; set; }
    public string ProductDescription { get; set; }
    public int TotalProduct { get; set; }
    public string HasDetails { get; set; }
    public string ProductOk { get; set; }
    public Color RColor { get; set; }
  }
}
