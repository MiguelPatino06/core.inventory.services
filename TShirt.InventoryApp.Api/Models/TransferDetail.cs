using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TShirt.InventoryApp.Api.Models
{
  public class TransferDetail
  {
    public int Id { get; set; }
    public string WarehouseOrigin { get; set; }
    public string WarehouseDestiny { get; set; }
    public string DateCreated { get; set; }
    public string Status { get; set; }
    public string Observation { get; set; }
    [NotMapped]
    public List<ProductTransferDetailExtend> products { get; set; }
  }
}