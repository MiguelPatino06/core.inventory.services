using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models.Xml;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
  public class XmlController : ApiController
  {
      [HttpGet]
      public IHttpActionResult XmlWrite(string documentType, int id)
      {
          XmlCountPlan countPlan = new XmlCountPlan();
          bool result = false;
          switch (documentType.ToUpper())
          {
              case "COUNTPLAN":

                  result = countPlan.Create(id); 

                  break;
              case "RECEPTION":
                  break;
              default:
                  return NotFound();
          }
          return Ok(result);
      }
  }
}
