using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
  public class OutputController : ApiController
  {
    private readonly IOutputRepository _outputRepository = new OutputRepository();

    [HttpGet]
    public IHttpActionResult Get(int id)
    {
      var result = _outputRepository.GetById(id);
      if (result != null)
        return Ok(result);
      else
        return NotFound();
    }

    [HttpGet]
    public IEnumerable<Output> GetList(int quantity, int? id)
    {
      if (id != null)
        return _outputRepository.GetListById(id.Value);
      else
        return _outputRepository.GetList(quantity);
    }

    [HttpPost]
    public int Save(Output output)
    {
      return _outputRepository.Save(output);
    }

  }
}
