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
    public class ProviderController : ApiController
    {

        private readonly IProviderRepository _proRepository = new ProviderRepository();

        [HttpGet]
        [ActionName("GetProvider")]
        public IHttpActionResult GetProvider(string code)
        {
            var result = _proRepository.GetByCode(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet]
        public IEnumerable<OrderTShirt> GetProviderOrder(string code)
        {
            return _proRepository.GetOrdersByProviderCode(code);
           
        }

        [HttpGet]
        [ActionName("GetProviderName")]
        public IEnumerable<Provider> GetProviderName(string name)
        {
            return _proRepository.GetProviderByName(name);

        }

    }
}
