using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;
using SportsStore.Shared.ViewModel;
using SportsStore.WebUI.Controllers;
/*****************************************************************************
*项目名称:SportsStore.UnitTests.Controllers
*项目描述:
*类 名 称:ShopCartControllerTests
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 21:43:25
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers.Tests
{
    [TestClass()]
    public class ShopCartControllerTests
    {
        [TestMethod()]
        public void Can_Add_To_ShopCart()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1",Category="Apples"},
            }.AsQueryable());
            ShopCart cart = new ShopCart();
            ShopCartController controller = new ShopCartController(mock.Object);
            //动作
            controller.AddToShopCart(cart, 1, null);
            //断言
            Assert.AreEqual(cart.Lines.Count, 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod()]
        public void Can_Goto_ShopCartPage_In_adding()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1",Category="Apples"}
            }.AsQueryable());
            ShopCart cart = new ShopCart();
            ShopCartController target = new ShopCartController(mock.Object);
            //动作
            RedirectToRouteResult result = target.AddToShopCart(cart, 2, "myUrl");
            //断言
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod()]
        public void Can_View_Index()
        {
            //准备
            ShopCart cart = new ShopCart();
            ShopCartController target = new ShopCartController(null);
            //动作
            ShopCartIndexViewModel result = (ShopCartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            //断言
            Assert.AreEqual(result.ShopCart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}