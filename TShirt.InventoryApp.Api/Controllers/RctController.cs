using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
    public class RctController : ApiController
    {
        private readonly IRctRepository _rctRepository = new RctRepository();

        [System.Web.Mvc.HttpGet]
        public IEnumerable<Rct> GetAll()
        {
            return _rctRepository.GetAll();
        }

        [System.Web.Mvc.HttpGet]
        public IHttpActionResult GetRct(string code)
        {
            var result = _rctRepository.GetByCode(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [System.Web.Mvc.HttpDelete]
        public HttpResponseMessage DeleteRct(int id)
        {
            try
            {
                var result = _rctRepository.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message.ToString());

            }

        }

        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage PostRct(RctExtendModel rct)
        {
            if (rct != null)
            {
                var result = _rctRepository.Add(rct);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}