using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("OrderTShirt")]
    public class OrderTShirt
    {
   
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

  
        public string Code { get; set; }


        public string Description { get; set; }


        public string ProviderCode { get; set; }

  
        public string Value1 { get; set; }

        public string Value2 { get; set; }


        public string Value3 { get; set; }

        public string Value4 { get; set; }


        public string Value5 { get; set; }

        [SQLite.Net.Attributes.Ignore]
        public Nullable<bool> IsSelected { get; set; }

        [OneToMany]
        public List<OrderDetail> Details { get; set; }
    }
}