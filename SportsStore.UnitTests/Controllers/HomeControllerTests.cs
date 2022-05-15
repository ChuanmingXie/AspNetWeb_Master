using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;
using SportsStore.Shared.ViewModel;
/*****************************************************************************
*项目名称:SportsStore.UnitTests.Controllers
*项目描述:
*类 名 称:HomeControllerTests
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/14 18:50:06
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/

namespace SportsStore.WebUI.Controllers.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"},
                new Product{ ProductID=3,Name="P3"},
                new Product{ ProductID=4,Name="P4"},
                new Product{ ProductID=5,Name="P5"},
                new Product{ ProductID=6,Name="P6"},
            });
            var controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            //动作
            IEnumerable<Product> result = (IEnumerable<Product>)controller.IndexNoPage(2).Model;

            //断言
            Product[] products = result.ToArray();
            Assert.IsTrue(products.Length == 3);
            Assert.AreEqual(products[0].Name, "P4");
            Assert.AreEqual(products[1].Name, "P5");
        }

        [TestMethod()]
        public void Can_Paginate_PageLiks()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1"},
                new Product{ProductID=2,Name="P2"},
                new Product{ProductID=3,Name="P3"},
                new Product{ProductID=4,Name="P4"},
                new Product{ProductID=5,Name="P5"},
            });
            var controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            //动作
            ProductsListViewModel result = (ProductsListViewModel)controller.Index(2).Model;

            //断言
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }
    }
}