using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TShirt.InventoryApp.Api.Models
{
  [SQLite.Net.Attributes.Table("ProductTransfer")]
  public class ProductTransfer
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string WarehouseOrigin { get; set; }
    public string WarehouseDestiny { get; set; }
    public string DateCreated { get; set; }
    public string Status { get; set; }
    public string Observation { get; set; }
    [NotMapped]
    public List<ProductTransferDetail> products { get; set; }
  }
}
