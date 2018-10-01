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
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        [ForeignKey(typeof(Rol))]
        public int RolId { get; set; }

        public string Observation { get; set; }

        public string DateCreated { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public string Value3 { get; set; }

        public string Value4 { get; set; }

        public string Value5 { get; set; }

        public string Password { get; set; }

    
    }
}