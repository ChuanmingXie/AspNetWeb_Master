using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;
using Mvc5.Knowleadge.Infrastructure;

namespace Mvc5.Knowleadge
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 7.使用约束：
            routes.MapRoute("ChromeRoute", "{*catchall}",
                new { contoller = "Home", action = "Index" },
                new { customConstraint = new UserAgentConstraint("Chrome") },
                new []{ "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });


            routes.MapRoute("UseConstraint", "{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new
                {
                    controller = "^H.*", action = "^Index$|^About$"
                    , httpMethod = new HttpMethodConstraint("GET")
                    , id = new RangeRouteConstraint(10, 20)
                },
                new[] { "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });

            routes.MapRoute("UseConstraint", "{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new
                {
                    controller = "^H.*", action = "^Index$|^About$"
                    , httpMethod = new HttpMethodConstraint("GET")
                    , id = new CompoundRouteConstraint(new IRouteConstraint[] { 
                        new AlphaRouteConstraint(),
                        new MinLengthRouteConstraint(6)
                    })
                },
                new[] { "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });

            // 3.使用静态片段
            routes.MapRoute("ShopSchema2", "Shop/OldAction", new { controller = "Home", action = "Index" });
            routes.MapRoute("ShopSchema", "Shop/{action}", new { controller = "Home" });
            routes.MapRoute("StaticExample2", "X{controller}/{action}");
            routes.MapRoute("StaticExample1", "Public/{controller}/{action}", new { controller = "Home", action = "Index" });

            // 4.自定义片段
            routes.MapRoute("DefinedPart", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "DefaultId" });
            routes.MapRoute("DefinedPartOptional", "{controller}/{action}/{id}", new { conttoller = "Home", action = "Index", id = UrlParameter.Optional });

            // 2.注册路由 与 使用默认值-方式2：
            routes.MapRoute("MyRoute", "{controller}/{action}", new { controller = "Home", action = "Index" });

            // 1.注册路由-方式1：
            //Route myRoute = new Route("{controller}/{action}", new MvcRouteHandler());
            //routes.Add("MyRoute", myRoute);

            routes.MapRoute(
            name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
            );

            // 5.可变长路由
            routes.MapRoute("DefinedLengthen", "{controller}/{action}/{id}/{*catchall}"
                , new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            // 6.在路由中使用命名空间
            //Route myRoute = routes.MapRoute("Default", "{controller}/{action}/{id}",
            //     new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //     namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
            // );
            //myRoute.DataTokens["UseNamespaceFallback"] = false;
        }
    }
}
