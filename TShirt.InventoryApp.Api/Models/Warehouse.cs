using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using System.Data.SQLite;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
  [Table("WareHouse")]
  public class Warehouse
  {
    [PrimaryKey]
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
  }
}