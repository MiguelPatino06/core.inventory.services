using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Services.Models;

namespace TShirt.InventoryApp.Web.Security
{
    public class SecurityUtils
    {
        public static UserSessionDataModel GetUserAuthenticated()
        {
            try
            {
                return HttpContext.Current.Session["UserData"] as UserSessionDataModel;

            }
            catch (Exception aException)
            {
                //TODO do some loggin
                return null;

            }
        }

        public static void SetUserAuthenticated(UserSessionDataModel aUser)
        {
            try
            {
                HttpContext.Current.Session["UserData"] = aUser;
            }
            catch (Exception aException)
            {

                //TODO do some loggin
                return;

            }


        }

        public static bool IsUserAuthorized()
        {
            //Check if user is authenticated
            var _AuthenticatedUser = GetUserAuthenticated();
            if (_AuthenticatedUser == null)
                return false;

            //TODO create logic for checking when users has permission to the area

            return true;

        }


    }
}