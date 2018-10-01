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
    public class UserController : ApiController
    {

        private readonly IUserRepository _userRepository = new UserRepository();

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        [HttpGet]
        public IHttpActionResult GetUser(string code, string pass)
        {
            var result = _userRepository.GetByCode(code, pass);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }


    }
}
