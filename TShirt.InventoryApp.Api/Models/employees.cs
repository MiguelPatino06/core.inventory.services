using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    
    [Table("employees")]
    public class employees
    {
        [PrimaryKey]
        public int employee_id { get; set; }

        public string last_name { get; set; }

        public string first_name { get; set; }

        [ForeignKey(typeof(departments))]
        public int department_id { get; set; }
    }
}