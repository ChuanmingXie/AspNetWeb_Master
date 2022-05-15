/*****************************************************************************
*项目名称:SportsStore.WebUI.Infrastructure.ModelBinder
*项目描述:
*类 名 称:ShopCartBinder
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 21:16:16
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
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