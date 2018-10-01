using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IProductRepository _productRepository = new ProductRepository();

        [HttpGet]
        public IHttpActionResult Search(string code)
        {
            var result = _productRepository.Search(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }
    }
}