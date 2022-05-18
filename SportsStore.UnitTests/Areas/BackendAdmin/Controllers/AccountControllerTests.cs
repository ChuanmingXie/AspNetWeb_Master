using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.DataInterface;
using SportsStore.Shared.Entities;
using SportsStore.Shared.ViewModel;
using SportsStore.WebUI.Areas.BackendAdmin.Controllers;
/*****************************************************************************
*项目名称:SportsStore.UnitTests.Areas.BackendAdmin.Controllers
*项目描述:
*类 名 称:AccountControllerTests
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/18 10:34:47
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

namespace SportsStore.WebUI.Areas.BackendAdmin.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        [TestMethod()]
        public void Can_Login_With_Valid_Credentials()
        {
            //准备
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "serect")).Returns(true);
            LoginVIewModel model = new LoginVIewModel
            {
                UserName = "admin",
                Password = "serect"
            };
            AccountController target = new AccountController(mock.Object);
            //动作
            ActionResult result = target.Login(model, "/MyURL");
            //断言
            Assert.IsInstanceOfType(result,typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_InValid_Credentials()
        {
            //准备
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUserName", "badPass")).Returns(false);
            LoginVIewModel model = new LoginVIewModel
            {
                UserName = "badUserName",
                Password = "badPass"
            };
            AccountController target = new AccountController(mock.Object);
            //动作
            ActionResult result = target.Login(model, "/MyURL");
            //断言
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }


        [TestMethod()]
        public void Can_Retrieve_Image_Data()
        {
            Product product = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1"},
                product,
                new Product{ProductID=3,Name="P3"},
            }.AsQueryable());

            ProductsController target = new ProductsController(mock.Object);
            ActionResult result = target.GetImage(2);
            //断言
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(product.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod()]
        public void Cannot_Retrieve_Image_Data_For_InValid_ID()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1"},
                new Product{ProductID=2,Name="P2"},
            }.AsQueryable());
            ProductsController target = new ProductsController(mock.Object);
            ActionResult result = target.GetImage(100);
            //断言
            Assert.IsNull(result);
        }
    }
}