using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace Mvc5.Knowleadge.Tests
{
    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        public void TestIncomingRoutes()
        {
            TestRouteMatch("~/Admin/Index", "Admin", "Index");
            TestRouteMatch("~/One/Two", "One", "Two");
            TestRouteMatch("~/Admin", "Admin", "Index");
            TestRouteFail("~/Admin/Index/Segment/3");
            //TestRouteFail("~/Admin"); 默认路由为删除可以匹配
        }

        [TestMethod]
        public void TestIncomingRoutesDefault()
        {
            TestRouteMatch("~/", "Home", "Index");  //无片段时,可以匹配到controller是Home;action是Index
            TestRouteMatch("~/Customer", "Customer", "Index");
            TestRouteMatch("~/Customer/List", "Customer", "List");
            TestRouteFail("~/Customer/List/All/3");
        }

        [TestMethod]
        public void TestIncomingRoutesStatic()
        {
            TestRouteMatch("~/XCustomer/List", "Customer", "List");
            TestRouteMatch("~/Public/Customer/List", "Customer", "List");
            TestRouteMatch("~/Shop/Index", "Home", "Index");
            TestRouteMatch("~/Shop/OldAction", "Home", "Index");
            TestRouteFail("~/Customer/List/All/3");
        }

        private HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            // 准备 请求
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            // 准备 响应
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            // 准备 具备 请求和响应的 上下文
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);
            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action
            , object routeProperties = null, string httpMethod = "GET")
        {
            // 准备
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //动作
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            //断言
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteTesult(result, controller, action, routeProperties));
        }

        private bool TestIncomingRouteTesult(RouteData routeResult, string controller, string action, object routeProperties)
        {
            Func<object, object, bool> valCompare = (v1, v2) =>
              {
                  return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
              };
            bool result = valCompare(routeResult.Values["controller"], controller)
                && valCompare(routeResult.Values["action"], action);
            if (routeProperties != null)
            {
                PropertyInfo[] propInfo = routeProperties.GetType().GetProperties();
                foreach (var pi in propInfo)
                {
                    if (!(routeResult.Values.ContainsKey(pi.Name)
                        && valCompare(routeResult.Values[pi.Name], pi.GetValue(routeProperties, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        private void TestRouteFail(string url)
        {
            //准备
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            //动作
            RouteData result = routes.GetRouteData(CreateHttpContext(url));
            //断言
            Assert.IsTrue(result == null || result.Route == null);
        }
    }
}
