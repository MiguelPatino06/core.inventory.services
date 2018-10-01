using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;


namespace TShirt.InventoryApp.Api.Models
{
    [SQLite.Net.Attributes.Table("DocumentDetails")]
    public  class DocumentDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [SQLiteNetExtensions.Attributes.ForeignKey(typeof(Document))]
        public int DocumentId { get; set; }
        public string ProductCode { get; set; }
        public string Quantity { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        [NotMapped]
        public string ProductName { get; set; }

    }
}