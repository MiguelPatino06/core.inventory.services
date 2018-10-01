using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TShirt.InventoryApp.Web.Security;

namespace TShirt.Inventory.App.Controllers
{
    [AppAccessFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


    }
}