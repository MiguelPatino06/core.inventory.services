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
    public class RolController : ApiController
    {

        private readonly IRolRepository _rolRepository = new RolRepository();

        [HttpGet]
        public IEnumerable<Rol> GetAll()
        {
            return _rolRepository.GetAll();
        }

        [HttpGet]
        public IHttpActionResult GetRol(int id)
        {
            var result = _rolRepository.GetById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpDelete]
        public HttpResponseMessage DeleteRol(int id)
        {
            try
            {
                var result = _rolRepository.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message.ToString());

            }
           
        }

        [HttpPost]
        public HttpResponseMessage PostRol(Rol rol)
        {
            if (rol != null)
            {
                var result = _rolRepository.Add(rol);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

    }
}
