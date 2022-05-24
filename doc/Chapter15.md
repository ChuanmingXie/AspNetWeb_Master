## 1. 路由的由来
### i.WebForm的路由
    在引入MVC框架之前,WebForm技术在URL请求上应用的是 ASP.NET 平台设定的URL与服务器磁盘上的文件之间有直接的关系。
    简单来讲就是WebForm技术在Http请求上运行的ASPX页面就是一个磁盘文件，同时也是对请求自包含的响应。
### ii.Mvc的路由
    Mvc框架下，请求时由控制器类中的动作方法处理的，而且与磁盘上的文件没有一一对应关系。为了处理Mvc的URL，
    ASP.NET平台使用了路由系统。路由系统用以创建所期望的任何URL模式。路由系统有两个功能：
        映射 输入URL（Incoming URL）：推断出该请求想要的是哪一个控制器Controller和动作Action
        生成 输出URL（Outgoing URL）：在视图渲染时生成URL，方便用户触发链接时，调用特定动作Action

## 2. URL模式(格式)
### i. 相关概念
    结构或方案：路由系统用一组路由来实现它的功能。这些路由共同组成了应用程序的URL架构(Schema)或方案(Scheme)。
        而URL架构(Schema)或方案(Scheme)是APP能够识别并作出响应的一组URL。
    模式：每条路由包含一个URL模式(Pattern),模式由几个片段组合成特定格式
    模式匹配：Mvc APP通常会有几条路由,路由系统会逐条与每个路由的URL模式相比较
### ii. URL模式特点
    对模式是保守的：模式只会匹配片段相同URL
    对片段是宽松的：URL格式与片段数相同时,模式不加验证直接将值对应到变量

## 3. 使用入站路由
[RouteConfig.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/Mvc5.Knowleadge/App_Start/RouteConfig.cs "RouteConfig.cs")
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

### ii. 定义路由默认值    
#### A. 设置路由默认值的目的
    使用默认路由主要是为了：优化路由系统URL<strong>对模式是保守的<strong>特点。即当传递的URL不包含任何片段时使用默认值。
    默认路由在框架原始的路由方案 RegistersRoutes静态方法 中已经通过defaults表达的十分清晰
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            namespaces: new string[]{ "Mvc5.Knowleadge.Controllers" }
        );
#### B. 为上文的简单路由添加默认值
    routes.MapRoute("MyRoute", "{controller}/{action}", new { controller = "Home", action = "Index" });
#### C. 为含有默认值的路由方案添加单元测试
    public void TextIncomingRoutesDefault()
    {
        TestRouteMatch("~/", "Home", "Index");  //无片段时,可以匹配到controller是Home;action是Index
        TestRouteMatch("~/Customer", "Customer", "Index");
        TestRouteMatch("~/Customer/List", "Customer", "List");
        TestRouteFail("~/Customer/List/All/3");
    }

### iii. 使用静态URL片段
#### A. 静态路由使用示例
    注意：越具体的路由方案越往前放置
    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

    routes.MapRoute("ShopSchema2", "Shop/OldAction", new { controller = "Home", action = "Index" });
    routes.MapRoute("ShopSchema", "Shop/{action}", new { controller = "Home" });
    routes.MapRoute("StaticExample2", "X{controller}/{action}");
    routes.MapRoute("StaticExample1", "Public/{controller}/{action}", new { controller = "Home", action = "Index" });

#### B. 使用静态路由的目的
    如果已经发布了URL有关的API方案，其他API使用者已经形成契约，就需要创建静态路由作为别名：
    routes.MapRoute("ShopSchema", "Shop/{action}", new { controller = "Home" });
    routes.MapRoute("ShopSchema2", "Shop/OldAction", new { controller = "Home", action = "Index" });
#### C. 测试静态路由
    public void TestIncomingRoutesStatic()
    {
        TestRouteMatch("~/XCustomer/List", "Customer", "List");
        TestRouteMatch("~/Public/Customer/List", "Customer", "List");
        TestRouteMatch("~/Shop/Index", "Home", "Index");
        TestRouteMatch("~/Shop/OldAction", "Home", "Index");
        TestRouteFail("~/Customer/List/All/3");
    }

### iv. 自定义片段变量
    除了controller和action片段变量之外，还可以自定义自己的变量。下面的示例演示了新增id片段的路由方案。 
    // 自定义片段
    routes.MapRoute("DefinedPart", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "DefaultId" });

    同时可以在视图中显示请求URL的id片段值
    - 控制器：

        public ActionResult Index()
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";
            ViewBag.CustomVariable = RouteData.Values["id"];
            return View("ActionName");
        }

    - 视图
    <div>控制器名称：@ViewBag.Controller</div>
    <div>动作的名称：@ViewBag.Action</div>
    <div>自定义片段：@ViewBag.CustomVariable</div>

    测试
    public void TestIncomingRoutesPart()
    {
        TestRouteMatch("~/", "Home", "Index", new { id = "DefaultId" });
        TestRouteMatch("~/Customer", "Customer", "Index", new { id = "DefaultId" });
        TestRouteMatch("~/Customer/List", "Customer", "List", new { id = "DefaultId" });
        TestRouteMatch("~/Customer/List/All", "Customer", "List", new { id = "All" });
        TestRouteFail("~/Customer/List/All/Delete");
    }
#### A. 自定义动作方法参数
    将URL模式中的片段变量名称作为控制器内动作方法的参数 相较于 使用RouteData.Values属性访问自定义路由变量 更显优雅。
    同时也更具有编程意义。
    控制器
    public ActionResult CustomVariable(string id)
    {
        ViewBag.Controller = "Home";
        ViewBag.Action = "Index";
        ViewBag.CustomVariable = id;
        return View("ActionName");
    }
#### B. 定义可选URL片段 ： id = UrlParameter.Optional
##### a.路由方案
    routes.MapRoute("DefinedPartOptional", "{controller}/{action}/{id}", new { conttoller = "Home", action = "Index", id = UrlParameter.Optional });
##### b.控制器
    public ActionResult CustomVariableOptional(string id)
    {
        ViewBag.Controller = "Home";
        ViewBag.Action = "Index";
        ViewBag.CustomVariable = id ?? "没有id值";
        return View("ActionName");
    }
##### c. 使用可选URL片段强制关注分离
    public ActionResult CustomVariableOptional(string id="DefaultId")
    {
        ViewBag.Controller = "Home";
        ViewBag.Action = "Index";
        ViewBag.CustomVariable = id;
        return View("ActionName");
    }

#### C. 定义可变长路由：{*catchall}
    全匹配：
    对模式是保守的特点可以通过接收可变数目的URL片段进行优化，通过指定一个全匹配(catchcall)的片段变量,
    以（*）号作为其前缀，可以对可变片段进行支持。
    routes.MapRoute("DefinedLengthen", "{controller}/{action}/{id}/{*catchall}"
    , new { controller = "Home", action = "Index", id = UrlParameter.Optional });

#### D. 设置路由命名空间
##### a.设置命名空间的原因
    当一个输入请求URL与一条路由进行匹配时,MVC框架根据controller的值(如 Home)查找控制器(如 HomeController)，
    但是我们无法避免在一个项目内有多个相同的HomeController位于不同的命名空间内，此时就需要在路由方案中使用命名空间。
    在具备区域Area的项目中，设置路由的命名空间非常重要。
##### b.示例：
* 解析主命名空间
    routes.MapRoute("Default", "{controller}/{action}/{id}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
    );
* 解析多路由控制命名空间
    routes.MapRoute("Default", "{controller}/{action}/{id}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        namespaces: new string[] { "Mvc5.Knowleadge.AdditionalControllers" }
    );
    routes.MapRoute("Default", "{controller}/{action}/{id}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
    );
    注意：不可以在一个路由方案中使用多个命名空间
##### c.禁用路由命名空间
    Route myRoute = routes.MapRoute("Default", "{controller}/{action}/{id}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        namespaces: new string[] { "Mvc5.Knowleadge.Controllers" }
    );
    myRoute.DataTokens["UseNamespaceFallback"] = false;

### v. 约束路由
    对模式保守的控制技术 —— 使用默认值、可选变量
    对片段宽松的控制技术 —— 约束
#### A. 使用正则表达式约束路由
    示例1：^ 只匹配controller片段以H字母打头的URL
    routes.MapRoute("UseRegx", "{controller}/{action}/{id}/{*catchall}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        new { controller = "^H.*" },
        new[] { "Mvc5.Knowleadge.Controllers" });

    示例2：| 会匹配action片段的值是Index和Aoubt的URL
    routes.MapRoute("UseRegx2", "{controller}/{action}/{id}/{*catchall}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        new { controller = "^H.*", action = "^Index$|^About$" },
        new[] { "Mvc5.Knowleadge.Controllers" });
#### B. 使用Http方法约束路由：httpMethod = new HttpMethodConstraint("GET")
* 添加约束
    routes.MapRoute("UseConstraint", "{controller}/{action}/{id}/{*catchall}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        new { controller = "^H.*", action = "^Index$|^About$", httpMethod = new HttpMethodConstraint("GET") },
        new[] { "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });
* 测试约束
    public void TestIncomingConstraint()
    {
        TestRouteMatch("~/Home/About/All/Delete/Parm", "Home", "About", new { id = "All", catchall = "Delete/Parm" });
        TestRouteFail("~/Home/OtherAction");
        TestRouteFail("~/Account/Index");
        TestRouteFail("~/Account/About");
    }
#### C. 使用数据类型和值范围约束： id = new RangeRouteConstraint(10, 20)
    routes.MapRoute("UseConstraint", "{controller}/{action}/{id}/{*catchall}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        new
        {
            controller = "^H.*", action = "^Index$|^About$"
            , httpMethod = new HttpMethodConstraint("GET")
            , id = new RangeRouteConstraint(10, 20)
        },
        new[] { "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });
    
    路由属性约束类
[路由属性类](https://blog.csdn.net/weixin_30421525/article/details/98389516 "路由属性")
    
    组合路由约束
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
#### D. 自定义约束
    实现接口IRouteConstraint
    // 以约束所用浏览器
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
    // 注册
    routes.MapRoute("ChromeRoute", "{*catchall}",
        new { contoller = "Home", action = "Index" },
        new { customConstraint = new UserAgentConstraint("Chrome") },
        new []{ "Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers" });

### vi. 属性路由
    Mvc框架同时支持约定路由和属性路由。
    Mvc模式的主要目标，将应用程序的不同部分分离，以使它们更容易编写、测试和维护。相较而言，
    约定路由使控制器不需要了解或依赖应用程序的路由配置而更适应这种模式。
    属性路由则将路由配置放置在动作或控制器名称之前，混和使用而显得不够分离
#### A. 使用属性路由:通过MapMvcAttributeRoutes扩展方法启用
    启用属性路由，需要在Routeconfig.cs的RegisterRoutes中添加 routes.MapMvcAttributeRoutes();
    在动作方法之前添加怕Route属性(如[Route("Test")])
        [Route("Test")]
        public ActionResult Index()
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";
            ViewBag.CustomVariable = RouteData.Values["id"];
            return View("ActionName");
        }
    访问URL https://localhost:port/Test

#### B. 使用片段变量创建路由
    示例
        [Route("Users/Add/{user}/{id}")]
        public string Create(string user,int id)
        {
            return $"User:{user},ID:{id}";
        }
    访问URL https://localhost:port/User/Add/Admin/100
#### C. 与约束路由组合使用
##### a. 示例：id:int 相当于 IntRouteConstraint
        [Route("Users/Add/{user}/{id:int}")]
        public string Create(string user, int id)
        {
            return $"创建动作 - 用户:{user},ID:{id}";
        }
##### b. 示例：属性路由运用多个路由，采用冒号 : 进行连接
        [Route("Users/Add/{user}/{passowrd}")]
        public string ChangePass(string user, string password)
        {
            return $"改变密码动作 - User:{user},Passowrd:{password}";
        }

        [Route("Users/Add/{user}/{passowrd:alpha:length(6)}")]
        public string ChangePassWord(string user, string password)
        {
            return $"改变密码动作 - User:{user},Passowrd:{password}";
        }
#### D. Route Prefix
    使用 Route Prefix 定义一个运用在该控制器中所有路由的前缀，当很多动作方法都是用同样的根URL可使用此方法    
    [RoutePrefix("Users")]
    public class CustomerController : Controller
    {
        //action
    }
    同时如果某个动作不希望使用此前缀可以使用"~/" (如 [Route("~/Test")])

## 4. 使用出站路由
### i. 借助Html.ActionLink()辅助器方法说明URL的出站规则
#### A. 结合路由系统生成URL
##### a. 使用示例
    路由方案
    routes.MapRoute("MyRoute", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
    使用辅助器
    @Html.ActionLink("输出URL", "CustomVariable")
    该示例生成
    URL：https://localhost:44344/Home/CustomVariable
    Html: <a href="/Home/CustomVariable">输出URL</a>
#### B. 指定控制器
##### a. 使用示例
    @Html.ActionLink("指定控制器", "Index", "Admin")
##### b. 结合不同路由方式
    当使用下述约定路由方案
    context.MapRoute("NewRoute", "App/Do{action}", new { controller = "Admin"});
    示例最终生成
    URL：https://localhost:44344/App/DoIndex
    Html: <a href="/App/DoIndex">指定控制器</a>
    当使用下述属性路由方案
        [Route("TestAction")]
        public ActionResult Index()
        {
            ViewBag.Controller = "Admin";
            ViewBag.Action = "Index";
            return View("ActionName");
        }
    注意：使用属性路由时要开启 routes.MapMvcAttributeRoutes();
         在区域Area中使用要添加context.Routes.MapMvcAttributeRoutes();
    示例最终生成
    URL: https://localhost:44344/TestAction
    Html: <a href="/TestAction">指定控制器</a>
#### C. 传递参数
##### a. 使用示例1
    路由方案
    context.MapRoute("NewRoute", "App/Do{action}", new { controller = "Admin" });
    使用辅助器
    @Html.ActionLink("设定参数", "CustomVariable", "Admin", new { id = "Hello" }, null)
    结果  
    https://localhost:44344/App/DoCustomVariable?id=Hello
##### b. 使用实例2
    路由方案
    routes.MapRoute("MyRoute", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
    使用辅助器
    @Html.ActionLink("设定参数", "CustomVariable", new { id = "Hello" })
    结果  
    https://localhost:44344/Home/CustomVariable/Hello
#### D. 传递标签属性(样式，id等等)
##### a. 使用示例
    辅助器
    @Html.ActionLink("设定参数", "CustomVariable", new { id = "Hello" },new { id="myAnchorID",@class="myCSSClass"})
    结果
    <a class="myCSSClass" href="/Home/CustomVariable/Hello" id="myAnchorID">设定参数</a>
#### E. 其他方法
##### a. 生成全限定链接
* 示例
    @Html.ActionLink("设定参数", "CustomVariable","Home","https",
    "myserver.cloudwhales.com","myFragementName",
    new { id = "Hello" }, new { id = "myAnchorID", @class = "myCSSClass" })
* 结果
    <a class="myCSSClass" href="https://myserver.cloudwhales.com:44344/RoutesHighAttribute/Home/CustomVariable/Hello#myFragementName" id="myAnchorID">设定参数</a>
* 源代码
    public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes);
##### b. 生成纯URL：@Url.Action()
    @Url.Action("Index","Home",new{id="MyId"})
##### c. 在动作方法中生成纯URL: 
    Url.RouteUrl(new{controller="Home",action=""Index})
    RedirectToRoute(controller="Home",action="Index",id="MyID")
##### d. 根据特定属性路由生成URL
    每条路由方案都有对应的名字，所以可以使用Html.RouteLink()覆盖默认路由匹配。
    路由方案
    routes.MapRoute("MyRoute", "{controller}/{action}");
    routes.MapRoute("MyOtherRoute", "App/{action}", new { controller = "Home" });
    默认辅助方法的使用
    @Html.ActionLink("测试1","Index","Customer")
    生成: <a href="/Customer/Index">输出URL</a>
    指定特定路由方案的复制器方法使用
    @Html.RouteLink("测试2","MyOtherRoute","Index","Customer")
    生成: <a Length="8" href="/App/Index?Length=5">输出URL</a>
    补充：对应的属性路由也可以设置类似名称
    [Route("Add/{user}/{id:int}",Name="AddRoute")]


### ii. 定制路由规则
#### A. 创建自定义的RouteBase实现
    如果需要改变标准Route对象对URL进行匹配的方式。例如你不喜欢标准方式，或者应用程序迁移到Mvc框架时，对外提供的
### iii. 使用区域

### iv. 对磁盘进行路由请求
### v. 绕过路由系统 使用IgnoreRoute()
    routes.IgnoreRoute("Areas/Content/{filename}.html");
### vi. 最佳URL效果
    项目创建选择MVC模板时，已经默认了很好的路由方案，基本可以不用修改，数值URL知识为了让开发人员对架构的理解更清晰。
#### A. 使URL整洁和人性化
##### a. 示例
    例如当当网，对于衣服的裤装类型，进行品牌、尺码、裤长的筛选的URL
[裤装筛选](http://category.dangdang.com/cid4008152-a1%3A5275_1000131%3A4_3%3A4503024.html "品类筛选")
    简洁后 http://category.dangdang.com/pants/brand/Size/length
##### b.生成简介URL原则：
* 设计的URL应用以描述内容，热不应该显示应用程序的细节，例如 /Articles/AnnualReport.比/WebSize_V2/Articles/AnnualReport简洁
* 尽可能采用内容标日而不是ID号，如 /Articles/AnnualReport. 比  /Articles/24324 人性化. 如果两者都采用 /Articles/24324/AnnualReport可以改善搜索引擎排序
* 对Html文件尽量不使用扩展名.html 对资源文件jpg、zip等可适当设置MIME类型,对文档文件pdf最好使用对应格式
* 尽量创建一种具备层次感的URL /Products/Menswear/Shirts/Red
* 避免使用符号、代码、字符序列，需要使用单词分割字符串时,单词直接通过短线分割，如 my-great-article. 加号，下滑线甚至空格都是不友好的
* URL确定后尽量不要打破，防止链接失效，需要修改时通过永久重定向使用旧的URL方案
* 让URL具备一致性，简短、易于输入、可剪切且持久稳定，对外可以让网站更加形象化 
* URL默认不区分大小写

##### c. GET和POST的选择