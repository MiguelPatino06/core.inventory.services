using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
  public class WarehouseController : ApiController
  {
    private readonly IWarehouseRepository _warehouseRepository = new WarehouseRepository();

    [HttpGet]
    public IEnumerable<Warehouse> GetAllWarehouse()
    {
      return _warehouseRepository.GetAll();
    }

    [HttpGet]
    public IHttpActionResult GetWarehouse(int Id)
    {
      var result = _warehouseRepository.GetById(Id);
      if (result != null)
        return Ok(result);
      else
        return NotFound();
    }
  }
}
