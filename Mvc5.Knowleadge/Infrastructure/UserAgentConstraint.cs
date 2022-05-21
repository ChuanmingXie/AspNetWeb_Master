/*****************************************************************************
*项目名称:Mvc5.Knowleadge.Infrastructure
*项目描述:
*类 名 称:UserAgentConstraint
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/21 12:06:24
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Mvc5.Knowleadge.Infrastructure
{
    public class UserAgentConstraint:IRouteConstraint
    {
        private string requiredUserAgent;

        public UserAgentConstraint(string agentPrarm)
        {
            requiredUserAgent = agentPrarm;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.UserAgent != null && httpContext.Request.UserAgent.Contains(requiredUserAgent);
        }
    }
}