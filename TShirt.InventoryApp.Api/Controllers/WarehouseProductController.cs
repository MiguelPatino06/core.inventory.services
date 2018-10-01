using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
  public class WarehouseProductController : ApiController
  {
    private readonly IWarehouseProductRepository _warehouseProductRepository = new WarehouseProductRepository();

    public IEnumerable<WarehouseProduct> GetAllWarehouseProduct()
    {
      return _warehouseProductRepository.GetAll();
    }

    public IHttpActionResult GetWarehouseProductByCodes(string warehouseCode, string productCode)
    {
      var result = _warehouseProductRepository.GetByCodes(warehouseCode, productCode);
      if (result != null)
        return Ok(result);
      else
        return NotFound();
    }

    public IHttpActionResult GetByCriteria(string warehouseCode, string criteria)
    {
      var result = _warehouseProductRepository.GetListByString(warehouseCode, criteria);
      if (result != null)
        return Ok(result);
      else
        return NotFound();
    }
  }
}