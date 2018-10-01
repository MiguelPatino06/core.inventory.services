using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TShirt.Inventory.App.Startup))]
namespace TShirt.Inventory.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
