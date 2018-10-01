using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TShirt.InventoryApp.Services.Models;

namespace TShirt.InventoryApp.Web.Models
{
  public class ProductTransferModels
  {
    public ProductTransferModels() { }

    [Display(Name = "Destino")]
    [Required]
    public string warehouseDestiny { get; set; }
    [Required]
    public string warehouseOrigin { get; set; }
    public string warehouseDestinyName { get; set; }
    public List<Warehouse> warehouses;
    public string productCode { get; set; }
    public List<ProductToTransfer> products { get; set; }
    public string observation { get; set; }

  }
}