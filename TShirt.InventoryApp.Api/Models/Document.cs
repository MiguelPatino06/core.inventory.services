using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("Documents")]
    public class Document
    {
        
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ProcessType { get; set; }
        public string Lot { get; set; }
        public string Code { get; set; }
        public string DateCreated { get; set; }
        public string WarehouseO { get; set; }
        public string WarehouseD { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        [OneToMany()]
        public List<DocumentDetail> Details { get; set; }
    }
}