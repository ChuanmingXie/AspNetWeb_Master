using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;
using SportsStore.WebUI.Controllers;
/*****************************************************************************
*项目名称:SportsStore.UnitTests.Controllers
*项目描述:
*类 名 称:MenuControllerTests
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 15:21:49
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

namespace SportsStore.WebUI.Controllers.Tests
{
    [TestClass()]
    public class MenuControllerTests
    {
        [TestMethod()]
        public void Can_Create_Category()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1",Category="Apples"},
                new Product{ProductID=2,Name="P2",Category="Apples"},
                new Product{ProductID=3,Name="P3",Category="Plums"},
                new Product{ProductID=4,Name="P4",Category="Oranges"},
            });
            MenuController controller = new MenuController(mock.Object);

            //动作
            string[] results = ((IEnumerable<string>)controller.Index().Model).ToArray();
            //断言
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod()]
        public void Indicates_Selected_Category()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1",Category="Apples"},
                new Product{ProductID=2,Name="P2",Category="Oranges"},
            });
            MenuController controller = new MenuController(mock.Object);
            string categoryToSelect = "Apples";
            //动作
            string result = controller.Index(categoryToSelect).ViewBag.SelectedCategory;
            //断言
            Assert.AreEqual(categoryToSelect, result);
        }
    }
}