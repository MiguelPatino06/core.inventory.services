using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TShirt.InventoryApp.Web.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AppAccessFilter: AuthorizeAttribute
    {
        /// <summary>
        /// Check if an user is authenticated
        /// </summary>
        /// <param name="httpContext">the httpContext</param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return SecurityUtils.IsUserAuthorized();
        }

        /// <summary>
        /// Redirect to the defined view if not authenticated
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (SecurityUtils.GetUserAuthenticated() != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Access"},
                    {"action", "AccessDenied"}
                });
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Access" },
                                                                                        { "action", "Login" } });
            }

        }
    }
}