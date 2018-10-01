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
    public class SampleController : ApiController
    {
        private readonly ISampleRepository _sampleRepository = new SampleRepository();

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _sampleRepository.GetById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var result = _sampleRepository.GetList();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }


        [HttpPost]
        public int Save(Sample items)
        {
            return _sampleRepository.Save(items);
        }

        [HttpPost]
        public bool Updated(Sample items)
        {
            return _sampleRepository.Update(items);
        }
    }
}
