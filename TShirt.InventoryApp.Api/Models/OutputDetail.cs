using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
  [SQLite.Net.Attributes.Table("OutputDetail")]
  public class OutputDetail
  {
    public int Id { get; set; }
    public string Warehouse { get; set; }
    public string ProductCode { get; set; }
    public int Quantity { get; set; }
    public int OutputId { get; set; }
    [NotMapped]
    public string ProductDescription { get; set; }
    [NotMapped]
    public string WarehouseName { get; set; }
  }
}