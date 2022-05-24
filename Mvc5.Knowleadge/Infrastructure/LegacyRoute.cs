/*****************************************************************************
*项目名称:Mvc5.Knowleadge.Infrastructure
*项目描述:
*类 名 称:LegacyRoute
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/23 7:03:04
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc5.Knowleadge.Infrastructure
{
    public class LegacyRoute : RouteBase
    {
        private string[] urls;

        public LegacyRoute(params string[] urls)
        {
            this.urls = urls;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData result = null;
            string requestURL = httpContext.Request.AppRelativeCurrentExecutionFilePath;
            if (urls.Contains(requestURL, StringComparer.OrdinalIgnoreCase))
            {
                result = new RouteData(this, new MvcRouteHandler());                
                result.Values.Add("controller", "Legacy");
                result.Values.Add("action", "GetLegacyURL");
                result.Values.Add("legacyURL", requestURL);
            }
            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            VirtualPathData result = null;
            if (values.ContainsKey("legacyURL") && urls.Contains((string)values["legacyURL"], StringComparer.OrdinalIgnoreCase))
            {
                result = new VirtualPathData(this, new UrlHelper(requestContext).Content((string)values["legacyURL"]).Substring(1));
            }
            return result;
        }
    }
}