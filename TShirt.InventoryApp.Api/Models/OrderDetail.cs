using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(OrderTShirt))]
        public string OrderCode { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string DateCreated { get; set; }
        public int? InitQuantity { get; set; }

    }
}