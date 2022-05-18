using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;
using SportsStore.Shared.ViewModel;
using System.Web.Mvc;
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
            IEnumerable<Product> result = (IEnumerable<Product>)controller.IndexNoLink(2).Model;

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
            ProductsListViewModel result = (ProductsListViewModel)controller.IndexNoCategory(2).Model;

            //断言
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }

        [TestMethod()]
        public void Can_Filter_Products()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Category="Cat1",Name="P1"},
                new Product{ProductID=2,Category="Cat2",Name="P2"},
                new Product{ProductID=3,Category="Cat1",Name="P3"},
                new Product{ProductID=4,Category="Cat2",Name="P4"},
                new Product{ProductID=5,Category="Cat3",Name="P5"},
            });
            HomeController controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            //动作
            Product[] result = ((ProductsListViewModel)controller.Index("Cat2", 1).Model).Products.ToArray();

            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod()]
        public void Generate_Category_Specific_Product_Count()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1",Category="Cat1"},
                new Product{ProductID=2,Name="P2",Category="Cat2"},
                new Product{ProductID=3,Name="P3",Category="Cat1"},
                new Product{ProductID=4,Name="P4",Category="Cat2"},
                new Product{ProductID=5,Name="P5",Category="Cat3"},
            });
            HomeController target = new HomeController(mock.Object) { PageSize = 3 };
            //动作
            int result1 = ((ProductsListViewModel)target.Index("Cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)target.Index("Cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)target.Index("Cat3").Model).PagingInfo.TotalItems;
            int resultAll = ((ProductsListViewModel)target.Index(null).Model).PagingInfo.TotalItems;
            //断言
            Assert.AreEqual(result1, 2);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3, 1);
            Assert.AreEqual(resultAll, 5);
        }
    }
}