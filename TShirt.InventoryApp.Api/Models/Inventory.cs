using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Data.Linq.Mapping;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using System.Data.SQLite;

namespace TShirt.InventoryApp.Api.Models
{
    [Table(Name = "Inventory")]
    public class Inventory
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int Id { get; set; }

        [Column(Name = "EmpName", DbType = "VARCHAR")]
        public string EmpName { get; set; }

        [Column(Name = "Salary", DbType = "DOUBLE")]
        public double Salary { get; set; }

        [Column(Name = "Designation", DbType = "VARCHAR")]
        public string Designation { get; set; }
    }
}