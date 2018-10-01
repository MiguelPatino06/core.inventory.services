using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
  public interface IWarehouseProductRepository
  {
    WarehouseProduct Add(WarehouseProduct warehouseProduct);
    IEnumerable<WarehouseProduct> GetAll();
    WarehouseProduct GetByCodes(string warehouseCode, string productCode);
    IEnumerable<WarehouseProduct> GetListByString(string warehouseCode, string str);
    bool UpdateAmountByCodes(string warehouseOrigin, string warehouseDestiny, long quantity, string productCode);
    bool UpdateAmountByCode(string warehouse, long quantity, string productCode);
  }
}