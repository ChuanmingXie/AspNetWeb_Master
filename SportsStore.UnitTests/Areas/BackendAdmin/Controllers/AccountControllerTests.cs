using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Shared.DataInterface;
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
    }
}