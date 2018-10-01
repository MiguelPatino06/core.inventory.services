using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace TShirt.InventoryApp.Api.Controllers
{
    public class TestController : ApiController
    {
        // GET: Test
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("Hi, I'm Connect TSHIRT");
        }
    }
}