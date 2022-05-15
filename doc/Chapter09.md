## 1. 创建购物车
### i. 购物车实体模型
#### A. 创建购物车实体模型
#### B. 测试购物车实体模型

### ii. 购物车控制器
#### A. 创建购物车控制器
#### B. 添加“添加购物车”触发按钮

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
    定义接口
    实现接口
    注册接口
#### B. 完善购物车控制器
#### C. 处理验证与提示
    提交空购物车的错误验证
    完成订单处理的友好提示