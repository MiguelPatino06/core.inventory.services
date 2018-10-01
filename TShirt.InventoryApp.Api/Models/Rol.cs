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


    [Table("Rol")]
    public class Rol
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string DateCreated { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        [OneToMany]
        public List<User> RolUser { get; set; }

    }
}