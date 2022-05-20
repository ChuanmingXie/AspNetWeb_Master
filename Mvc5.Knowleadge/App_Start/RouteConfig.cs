using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc5.Knowleadge
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("ShopSchema2", "Shop/OldAction", new { controller = "Home", action = "Index" });
            routes.MapRoute("ShopSchema", "Shop/{action}", new { controller = "Home" });
            routes.MapRoute("StaticExample2", "X{controller}/{action}");
            routes.MapRoute("StaticExample1", "Public/{controller}/{action}", new { controller = "Home", action = "Index" });

            //方式1：
            //Route myRoute = new Route("{controller}/{action}", new MvcRouteHandler());
            //routes.Add("MyRoute", myRoute);
            //方式2：
            routes.MapRoute("MyRoute", "{controller}/{action}", new { controller = "Home", action = "Index" });

            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
            );
        }
    }
}
