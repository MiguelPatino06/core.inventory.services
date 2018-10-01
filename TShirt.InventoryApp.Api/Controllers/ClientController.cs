using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using TShirt.InventoryApp.Api.Services;
namespace TShirt.InventoryApp.Api.Controllers
{
    public class ClientController : ApiController
    {
        private readonly IClientRepository _clientRepository = new ClientRepository();

        [HttpGet]
        public IHttpActionResult Search(string code)
        {
            var result = _clientRepository.List(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetByCode(string code)
        {
            var result = _clientRepository.GetbyCode(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }
    }
}