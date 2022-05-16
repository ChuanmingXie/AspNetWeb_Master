## 1. 创建购物车
### i. 购物车实体模型
#### A. 创建购物车实体模型
[ShopCart.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.Shared/Entities/ShopCart.cs "购物车实体模型")
#### B. 测试购物车实体模型
[ShopCartTests.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.UnitTests/Entities/ShopCartTests.cs "测试购物车实体")
### ii. 购物车控制器
#### A. 创建购物车控制器
    初始版本的购物车控制器，借助Session会话对象存储选购的商品信息.
    private ShopCart GetShopCart()
    {
        ShopCart shopCart = (ShopCart)Session["ShopCart"];
        if (shopCart == null)
        {
            shopCart = new ShopCart();
            Session["ShopCart"] = shopCart;
        }
        return shopCart;
    }
[ShopCartController](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.WebUI/Controllers/ShopCartController_NoBinder.cs "初始版本控制器")
    控制器内包含：
        将商品添加至购物车方法 AddToShopCart
        将商品从购物车移除方法 RemoveFormShopCart
        以及购物车清单视图界面动作
#### B. 添加“添加购物车”选购按钮
    在产品列表的分部视图ProductSummary.cshtml内添加选购按钮，同时表单提交的动作指向购物车控制器
    @model SportsStore.Shared.Entities.Product
    <div class="well">
        <h3>
            <strong>@Model.Name</strong>
            <span class="pull-right label label-primary">@Model.Price.ToString("c")</span>
        </h3>
        @using (Html.BeginForm("AddToShopCart", "ShopCart"))
        {
            <div class="pull-right">
                @Html.HiddenFor(x => x.ProductID)
                @Html.Hidden("returnUrl", Request.Url.PathAndQuery)
                <input type="submit" class="btn btn-success" value="加入购物车" />
            </div>
        }
        <span class="lead">@Model.Description</span>
    </div>
### iii. 购物车清单视图
#### A. 创建购物车清单视图模型

#### B. 完善控制器
#### C. 创建购物车清单视图

## 2. 使用模型绑定
### i. 模型绑定含义
    作用：模型绑定在MVC框架中起到，通过HTTP请求创建C#对象的作用，目的时将这些C#对象作为参数值传递给动作方法。
    此理念在便饭处理上应用非常广泛。
    应用：在本例中ShopCart实体在处理时通过Session(会话状态)获取和存储，这种方式不利于单元测试，
    所以考虑将其转化为模型绑定的方式向外提供控制器接口参数

### ii. 自定义购物车模型绑定器
    在自定义模型绑定时注意使用命名空间在 using System.Web.Mvc; 下的IModelBander接口; 而不是 System.Web.ModelBinding;下的
    两者分别完成不同的绑定任务
    using System.Web.Mvc;
    using SportsStore.Shared.Entities;

    namespace SportsStore.WebUI.Infrastructure.ModelBinder
    {
        public class ShopCartBinder : IModelBinder
        {
            private const string sessionkey = "ShopCart";
            public object BindModel(ControllerContext executionContext, ModelBindingContext bindingContext)
            {
                ShopCart shopCart = null;
                if (executionContext.HttpContext.Session != null)
                {
                    shopCart = (ShopCart)executionContext.HttpContext.Session[sessionkey];
                }
                if (shopCart == null)
                {
                    shopCart = new ShopCart();
                    if (executionContext.HttpContext.Session != null)
                    {
                        executionContext.HttpContext.Session[sessionkey] = shopCart;
                    }
                }
                return shopCart;
            }
        }
    }

### iii. 使用购物车模型模型绑定器
#### A. 配置自定义模型绑定
    在Global.asax文件中配置模型绑定
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(ShopCart), new ShopCartBinder());
        }
    }
#### B. 修改购物车控制器

## 3. 完成购物车
### i. 添加“删除物品”按钮

### ii. 添加购物车清单摘要

## 4. 订单提交与处理
### i. 购物车结算
#### A. 添加购物车结算模型
#### B. 添加购物车结算动作
#### C. 添加购物车结算视图
    初始版本
    优化版本

### ii. 订单处理-以发送邮箱为示例
#### A. 接口实现与注册
##### a. 定义接口
    public interface IOrderProcessor
    {
        void ProcessOrder(ShopCart shopCart, ShoppingDetails details);
    }
##### b. 实现接口
    实现QQ邮箱发送邮件
[EmailOrderProcessor](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.Shared/Services/EmailOrderProcessor.cs "QQ邮箱发送邮件")
    实现Outlook邮箱发送邮件
[EmailOutlookProcessor](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.Shared/Services/EmailOutlookProcessor.cs "QQ邮箱发送邮件")
##### c. 注册接口
    NinjectWebCommon文件添加
    EmailSettings emailSettings = new EmailSettings
    {
        WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteFile"] ?? "false")
    };
    kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
    //kernel.Bind<IOrderProcessor>().To<EmailOutlookProcessor>().WithConstructorArgument("settings", emailSettings);
    Web.config文件
    <appSettings>
        ...
        <add key="Email.WriteAsFile" value="true"/>
	</appSettings>
    
##### b. 问题总结：
    QQ邮箱开启权限验证
        问题：Error: need EHLO and AUTH first
        原因:
        处理:
    outlook邮箱系统设置
        问题：
        Retrieving the COM class factory for component with CLSID {0006F03A-0000-0000-C000-000000000046} failed due to the following error: 80080005 服务器运行失败 (Exception from HRESULT: 0x80080005 (CO_E_SERVER_EXEC_FAILURE)).
        Description: An unhandled exception occurred during the execution of the current web request. Please review the stack trace for more information about the error and where it originated in the code.

        Exception Details: System.Runtime.InteropServices.COMException: Retrieving the COM class factory for component with CLSID {0006F03A-0000-0000-C000-000000000046} failed due to the following error: 80080005 服务器运行失败 (Exception from HRESULT: 0x80080005 (CO_E_SERVER_EXEC_FAILURE)).
        处理:

#### B. 完善购物车控制器
#### C. 处理验证与提示
    提交空购物车的错误验证
    完成订单处理的友好提示