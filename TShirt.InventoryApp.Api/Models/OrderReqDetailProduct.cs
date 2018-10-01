using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("OrderReqDetailProduct")]
    public class OrderReqDetailProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string OrderReqCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductCodeChanged { get; set; }
        public int Quantity { get; set; }
        public string DateProductChanged { get; set; }
        public string UserUpdated { get; set; }
        public int Status { get; set; }
        public string Warehouse { get; set; }
    }
}