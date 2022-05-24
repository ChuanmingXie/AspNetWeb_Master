using System.Web.Mvc;
using System.Web.Routing;
using Mvc5.Knowleadge.Infrastructure;

namespace Mvc5.Knowleadge
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute("MyRoute", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            //routes.MapMvcAttributeRoutes();
            //routes.Add(new LegacyRoute("~/Legacy/GetLegacyURL", "~/RoutesHighAttribute/Legacy/GetLegacyURL"));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
            );

        }
    }
}
