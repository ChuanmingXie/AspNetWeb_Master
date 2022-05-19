## 1. 路由的由来
### i.WebForm的路由
    在引入MVC框架之前,WebForm技术在URL请求上应用的是 ASP.NET 平台设定的URL与服务器磁盘上的文件之间有直接的关系。简单来讲就是WebForm技术在Http请求上运行的ASPX页面就是一个磁盘文件，同时也是对请求自包含的响应。
### ii.Mvc的路由
    Mvc框架下，请求时由控制器类中的动作方法处理的，而且与磁盘上的文件没有一一对应关系。为了处理Mvc的URL，
    ASP.NET平台使用了路由系统。路由系统用以创建所期望的任何URL模式。路由系统有两个功能：
        映射 输入URL（Incoming URL）：推断出该请求想要的是哪一个控制器Controller和动作Action
        生成 输出URL（Outgoing URL）：在视图渲染时生成URL，方便用户触发链接时，调用特定动作

## 2. URL模式(格式)
### i. 相关概念
    结构或方案：路由系统用一组路由来实现它的功能。这些路由共同组成了应用程序的URL架构(Schema)或方案(Scheme)。
        而URL架构(Schema)或方案(Scheme)是APP能够识别并作出响应的一组URL。
    模式：每条路由包含一个URL模式(Pattern),模式由几个片段组合成特定格式
    模式匹配：Mvc APP通常会有几条路由,路由系统会逐条与每个路由的URL模式相比较
### ii. URL模式特点
    对模式保守：模式只匹配片段相同URL
    对片段宽松：URL格式与片段数相同时,模式不加验证直接将值对应到变量

## 3. 使用路由
### i. 注册一条简单路由并测试
#### A. 系统目录下Global.asax.cs内包含对Route.Config.cs文件去静态方法的调用
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    namespace Mvc5.Knowleadge
    {
        public class MvcApplication : HttpApplication
        {
            protected void Application_Start()
            {
                AreaRegistration.RegisterAllAreas();
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }
        }
    }
#### B. 在Route.Config.cs方法中,新添一条简单路由
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //方式1：
        //Route myRoute = new Route("{controller}/{action}", new MvcRouteHandler());
        //routes.Add("MyRoute", myRoute);
        //方式2：
        routes.MapRoute("MyRoute", "{controller}/{action}");

        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            namespaces: new string[]{ "Mvc5.Knowleadge.Controllers" }
        );
    }
#### C. 测试路由，新建一个全局的测试类用于测试路由系统
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

### ii. 定义默认路由

### iii. 使用静态URL片段

### vi. 自定义片段变量
#### A. 自定义动作方法参数
#### B. 定义可选URL片段
#### C. 定义可变长路由
#### D. 通过命名空间设定优先级

### v. 约束路由
#### A. 使用正则表达式约束路由
#### B. 使用Http方法约束路由
#### C. 使用数据类型和值范围约束
#### D. 自定义约束

### v. 属性路由
#### A. 使用属性路由
#### B. 使用片段变量创建路由
#### C. 与约束路由组合使用
#### D. Route Prefix
