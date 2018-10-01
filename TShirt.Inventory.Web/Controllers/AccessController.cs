using System.Threading.Tasks;
using System.Web.Mvc;
using TShirt.InventoryApp.Services.Services;
using TShirt.InventoryApp.Web.ViewModels;

namespace TShirt.InventoryApp.Web.Controllers
{
    public class AccessController : Controller
    {
        private UserServices _services;
        // GET: Access
        public ActionResult Login()
        {
            return View("Login");
        }


        //[HttpPost]
        //public ActionResult Login(FormCollection fm)
        //{

        //    return RedirectToAction("Index", "Client");

        //}

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginCredentials aModel)
        {


            _services = new UserServices();

            var _LoginResult = await _services.GetProviderName(aModel.Code, aModel.Password);
            if (_LoginResult == null)
            {
                ModelState.AddModelError("", "Error");
                return View("Login");
            }

            /*Codigo nuevo*/
            //Session["GymID"] = _LoginResult.GymId;
            //var authTicket = new FormsAuthenticationTicket(
            //        1,                             // version
            //        aModel.Email,                  // user name
            //        DateTime.Now,                  // created
            //        DateTime.Now.AddMinutes(20),   // expires
            //        true,                          // persistent?
            //        _LoginResult.RoleData[0].Name  // can be used to store roles
            //        );

            //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            //var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

            Session["UserSession"] = aModel.Code;

            ///*FIN Codigo nuevo*/

            //SecurityUtils.SetUserAuthenticated(_LoginResult);
            return RedirectToAction("Index", "ProductChange");

        }
    }
}