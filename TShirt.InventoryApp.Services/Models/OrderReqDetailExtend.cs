
namespace TShirt.InventoryApp.Services.Models
{
    public class OrderReqDetailExtend
    {
        public int Id { get; set; }
        public string OrderReqCode { get; set; }
        public string Observation { get; set; }
        public string ProductCode { get; set; }
        public string ProductCodeChanged { get; set; }
        public int? Quantity { get; set; }
        public string DateProductChanged { get; set; }
        public string UserUpdated { get; set; }
        public string ProductName { get; set; }
        public string ProductNameChanged { get; set; }
        public bool IsSelect { get; set; }
    }
}
