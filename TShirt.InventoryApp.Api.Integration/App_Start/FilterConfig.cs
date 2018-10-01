using System.Web;
using System.Web.Mvc;

namespace TShirt.InventoryApp.Api.Integration
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
