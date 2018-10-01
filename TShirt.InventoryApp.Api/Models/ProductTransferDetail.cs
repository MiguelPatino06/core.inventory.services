using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TShirt.InventoryApp.Api.Models
{
  [Table("ProductTransferDetail")]
  public class ProductTransferDetail
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string ProductCode { get; set; }
    public string ProductDescription { get; set; }
    public int ProductTransfer_Id { get; set; }
  }
}