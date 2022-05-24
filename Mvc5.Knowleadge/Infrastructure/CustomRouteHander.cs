/*****************************************************************************
*项目名称:Mvc5.Knowleadge.Infrastructure
*项目描述:
*类 名 称:CustomRouteHander
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/24 10:22:40
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
    public class CustomRouteHander : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CustomeHttpHandler();
        }
    }

    public class CustomeHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write("Hello");
        }
    }
}