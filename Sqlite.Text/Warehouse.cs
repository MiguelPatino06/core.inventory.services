using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Data.Linq.Mapping;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using System.Data.SQLite;

namespace Sqlite.Text
{
    [Table(Name = "WareHouse")]
    public class Warehouse
    {
        [Column(Name = "Id", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int Id { get; set; }

        [Column(Name = "Name", DbType = "VARCHAR")]
        public string Name { get; set; }
    }
}
