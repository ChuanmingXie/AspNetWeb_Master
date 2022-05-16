using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.DataInterface;
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
            ShopCartController controller = new ShopCartController(mock.Object,null);
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
            ShopCartController target = new ShopCartController(mock.Object, null);
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
            ShopCartController target = new ShopCartController(null, null);
            //动作
            ShopCartIndexViewModel result = (ShopCartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            //断言
            Assert.AreEqual(result.ShopCart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod()]
        public void Cannot_CheckOut_Empty_Cart()
        {
            //准备
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            ShopCart cart = new ShopCart();
            ShoppingDetails shoppingDetails = new ShoppingDetails();
            ShopCartController target = new ShopCartController(null, mock.Object);
            //动作
            ViewResult result = target.CheckOut(cart, shoppingDetails);
            //断言
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShopCart>(), It.IsAny<ShoppingDetails>()), Times.Never());
            Assert.AreEqual("",result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_CheckOut_Invalid_ShoppingDetails()
        {
            //准备
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            ShopCart cart = new ShopCart();
            cart.AddItemProduct(new Product(), 1);
            ShopCartController target = new ShopCartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");
            //动作
            ViewResult result = target.CheckOut(cart, new ShoppingDetails());
            //断言
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShopCart>(), It.IsAny<ShoppingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_CheckOut_And_Sumbit_Order()
        {
            //准备
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            ShopCart cart = new ShopCart();
            cart.AddItemProduct(new Product(), 1);
            ShopCartController target = new ShopCartController(null, mock.Object);
            //动作
            ViewResult result = target.CheckOut(cart, new ShoppingDetails());
            //断言
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShopCart>(), It.IsAny<ShoppingDetails>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}