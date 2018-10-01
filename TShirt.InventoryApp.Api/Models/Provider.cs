using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using System.Data.SQLite;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("Provider")]
    public class Provider
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Barcode { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public string Value3 { get; set; }

        public string Value4 { get; set; }

        public string Value5 { get; set; }
    }
}