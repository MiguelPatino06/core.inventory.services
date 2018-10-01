using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TShirt.InventoryApp.Api.Models
{
    public class ViewOrderExtend
    {
      
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ProviderCode { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string ProviderName { get; set; }
        public string ProviderBarcode { get; set; }
        public string ProductCode { get; set; }
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string BarcodeProduct { get; set; }
        public int Quantity { get; set; }
        public string OrderValue1 { get; set; }
        public string OrderValue2 { get; set; }
        public string OrderValue3 { get; set; }
        public string OrderValue4 { get; set; }
        public string OrderValue5 { get; set; }
        public int TotalProduct { get; set; }

    }
}