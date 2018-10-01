using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TShirt.InventoryApp.Web.Helpers
{
  public static class MyHelpers
  {
    public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                         string text, string title, string action,
                                         string controller,
                                         object routeValues = null,
                                         object htmlAttributes = null)
    {
      UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

      TagBuilder builder = new TagBuilder("a");
      builder.InnerHtml = text;
      builder.Attributes["title"] = title;
      builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
      builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

      return MvcHtmlString.Create(builder.ToString());
    }

    public static string StatusName(string code)
    {
      string name = "";

      switch (code)
      {
        case "0":
          name = "Pendiente";
          break;
        case "1":
          name = "Aprobada";
          break;
        case "2":
          name = "Rechazada";
          break;
      }

      return name;
    }


  }
}