using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TShirt.InventoryApp.Services.Properties;
using TShirt.InventoryApp.Web.Security;


namespace TShirt.Inventory.App.Utils
{
    public static class LoggedInUserHelpers
    {
        public static string UserName(this HtmlHelper aHelper)
        {
            return SecurityUtils.GetUserAuthenticated().UserFullName;
        }

        public static string MembershipDate(this HtmlHelper aHelper)
        {
            return string.Format(Resources.MembershipDisplayMessage, SecurityUtils.GetUserAuthenticated().UserCreationDate);
        }
    }
}