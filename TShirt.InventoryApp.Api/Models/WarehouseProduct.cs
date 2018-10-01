using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Models
{
  [SQLite.Net.Attributes.Table("WarehouseProduct")]
  public class WarehouseProduct
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string WarehouseCode { get; set; }
    public string ProductCode { get; set; }
    public decimal Quantity { get; set; }

    [NotMapped]
    public Warehouse Warehouse { get; set; }
    [NotMapped]
    public Product Product { get; set; }
  }
}