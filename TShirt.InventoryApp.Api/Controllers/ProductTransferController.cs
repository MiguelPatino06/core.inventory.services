using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
  public class ProductTransferController : ApiController
  {
    private readonly IProductTransferRepository _productTransferRepository = new ProductTransferRepository();

    [HttpGet]
    public IHttpActionResult Get(int id)
    {
      var result = _productTransferRepository.GetById(id);
      if (result != null)
        return Ok(result);
      else
        return NotFound();
    }

    [HttpPost]
    public HttpResponseMessage Save(ProductTransfer productTransfer)
    {
      if (productTransfer != null)
      {
        var result = _productTransferRepository.Save(productTransfer);
        return Request.CreateResponse(HttpStatusCode.OK, result);
      }
      else
      {
        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
      }
    }

    [HttpGet]
    public IEnumerable<TransferDetail> GetRequests(int? code)
    {
      if(code != null)
        return _productTransferRepository.GetRequests(code.Value);
      else
        return _productTransferRepository.GetRequests();
    }

  }
}