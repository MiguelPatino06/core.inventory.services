using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    //[System.Data.Linq.Mapping.Table(Name = "departments")]

    [Table("departments")]
    public class departments
    {
        [PrimaryKey]
        public int department_id { get; set; }
        public string department_name { get; set; }
        
        [OneToMany]
        public List<employees> Empleados { get; set; }
    }
}