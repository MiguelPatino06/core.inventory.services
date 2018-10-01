using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
   [Table("OrderReqDetail")]
    public class OrderReqDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string OrderReqCode { get; set; }
        public string Observation { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public string Warehouse { get; set; }

    }
}