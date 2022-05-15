using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SportsStore.Shared.Entities;
using SportsStore.WebUI.Infrastructure.ModelBinder;

namespace SportsStore.WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(ShopCart), new ShopCartBinder());
        }
    }
}
