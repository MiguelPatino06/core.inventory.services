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
    [Table("ProviderOrder")]
    public class ProviderOrder
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProviderBarCode { get; set; }
        public string OrderCode { get; set; }
        public string OrderDescription { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public string ProductBarCode { get; set; }
        public string ProductDescription { get; set; }

    }
}