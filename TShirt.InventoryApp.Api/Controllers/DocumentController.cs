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

    public class DocumentController : ApiController
    {

        private readonly IDocumentRepository _documentRepository = new DocumentRepository();

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _documentRepository.GetById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var result = _documentRepository.GetAll();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public int Save(Document items)
        {
            return _documentRepository.Save(items);
        }

    }
}
