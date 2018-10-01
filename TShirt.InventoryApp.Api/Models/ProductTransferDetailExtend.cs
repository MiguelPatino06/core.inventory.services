using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TShirt.InventoryApp.Api.Models
{
  public class ProductTransferDetailExtend
  {
    public long Quantity { get; set; }
    public string ProductCode { get; set; }
    public string ProductDescription { get; set; }
  }
}
