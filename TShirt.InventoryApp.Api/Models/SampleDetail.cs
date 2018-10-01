using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [SQLite.Net.Attributes.Table("SampleDetail")]
    public class SampleDetail
    {
        public int Id { get; set; }
        public int SampleId { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public string User { get; set; }
        public string DateCreated { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        
    }
}