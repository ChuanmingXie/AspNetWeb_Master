using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;
using Mvc5.Knowleadge.Infrastructure;

namespace Mvc5.Knowleadge.Areas.UrlsAndRoutes
{
    public class UrlsAndRoutesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UrlsAndRoutes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // 3.使用静态片段
            context.MapRoute("ShopSchema2", "UrlsAndRoutes/Shop/OldAction",
                new { controller = "Home", action = "Index" });
            context.MapRoute("ShopSchema", "UrlsAndRoutes/Shop/{action}",
                new { controller = "Home" });
            context.MapRoute("StaticExample2", "UrlsAndRoutes/X{controller}/{action}");
            context.MapRoute("StaticExample1", "UrlsAndRoutes/Public/{controller}/{action}",
                new { controller = "Home", action = "Index" });

            // 4.自定义片段
            context.MapRoute("DefinedPart", "UrlsAndRoutes/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "DefaultId" });
            context.MapRoute("DefinedPartOptional", "UrlsAndRoutes/{controller}/{action}/{id}",
                new { conttoller = "Home", action = "Index", id = UrlParameter.Optional });

            // 2.注册路由 与 使用默认值-方式2：
            context.MapRoute("MyRoute", "UrlsAndRoutes/{controller}/{action}",
                new { controller = "Home", action = "Index" });

            // 1.注册路由-方式1：
            //Route myRoute = new Route("{controller}/{action}", new MvcRouteHandler());
            //routes.Add("MyRoute", myRoute);

            // 5.可变长路由
            context.MapRoute("DefinedLengthen", "UrlsAndRoutes/{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            context.MapRoute(
                "UrlsAndRoutes_default",
                "UrlsAndRoutes/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            context.MapRoute("ChromeRoute", "UrlsAndRoutes/{*catchall}",
                new { contoller = "Home", action = "Index" },
                new { customConstraint = new UserAgentConstraint("Chrome") });

            // 6.在路由中使用命名空间
            //Route myRoute = routes.MapRoute("Default", "UrlsAndRoutes/{controller}/{action}/{id}",
            //     new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //     namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
            // );
            //myRoute.DataTokens["UseNamespaceFallback"] = false;

            // 7.使用约束：
            //context.MapRoute("UseConstraint", "UrlsAndRoutes/{controller}/{action}/{id}/{*catchall}",
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    new
            //    {
            //        controller = "^H.*", action = "^Index$|^About$"
            //        , httpMethod = new HttpMethodConstraint("GET")
            //        , id = new RangeRouteConstraint(10, 20)
            //    },
            //    new[] { "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });

            //context.MapRoute("UseConstraint2", "UrlsAndRoutes/{controller}/{action}/{id}/{*catchall}",
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    new
            //    {
            //        controller = "^H.*", action = "^Index$|^About$"
            //        , httpMethod = new HttpMethodConstraint("GET")
            //        , id = new CompoundRouteConstraint(new IRouteConstraint[] {
            //            new AlphaRouteConstraint(),
            //            new MinLengthRouteConstraint(6)
            //        })
            //    },
            //    new[] { "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });
        }
    }
}