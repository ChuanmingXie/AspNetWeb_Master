using System.Web.Mvc;
using System.Web.Routing;

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
            context.MapRoute(
                "UrlsAndRoutes_default",
                "UrlsAndRoutes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}