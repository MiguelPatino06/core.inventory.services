using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [SQLite.Net.Attributes.Table("Sample")]
    public class Sample
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
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
        [NotMapped]
        public List<SampleDetailExtend> Details { get; set; } 
    }
}