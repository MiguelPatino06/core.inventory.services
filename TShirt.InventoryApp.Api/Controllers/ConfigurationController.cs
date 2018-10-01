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
    public class ConfigurationController : ApiController
    {

        private readonly IConfigurationRepository _configurationRepository = new ConfigurationRepository();


        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var result = _configurationRepository.Get();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }


        [HttpPost]
        public bool Save(Configuration items)
        {
            return _configurationRepository.Save(items);
        }
    }
}
