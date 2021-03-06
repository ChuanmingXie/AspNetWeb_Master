/*****************************************************************************
*项目名称:SportsStore.UnitTests.Areas.BackendAdmin.Controllers
*项目描述:
*类 名 称:ProductsControllerTests
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/17 21:49:03
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;

namespace SportsStore.WebUI.Areas.BackendAdmin.Controllers.Tests
{
    [TestClass()]
    public class ProductsControllerTests
    {
        [TestMethod()]
        public void Index_Contains_All_ProductsAsync()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1"},
                new Product{ProductID=2,Name="P2"},
                new Product{ProductID=3,Name="P3"},
            });
            ProductsController target = new ProductsController(mock.Object);
            //动作
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();
            //断言
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod()]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1"},
                new Product{ProductID=2,Name="P2"},
                new Product{ProductID=3,Name="P3"},
            });
            ProductsController target = new ProductsController(mock.Object);
            //动作
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;
            //断言
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod()]
        public void Cannot_Edit_NoExist_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1"},
                new Product{ProductID=2,Name="P2"},
                new Product{ProductID=3,Name="P3"},
            });
            ProductsController target = new ProductsController(mock.Object);
            //动作
            Product result = (Product)target.Edit(4).ViewData.Model;
            //断言
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Can_Save_Valid_Changes()
        {
            //准备
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            ProductsController target = new ProductsController(mock.Object);
            Product product = new Product { Name = "Test" };
            //动作
            ActionResult result = target.Edit(product);
            //断言
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }


        [TestMethod()]
        public void Cannot_Save_InValid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            ProductsController target = new ProductsController(mock.Object);
            Product product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");
            //动作
            ActionResult result = target.Edit(product);
            //断言
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void Can_Delete_Valid_Products()
        {
            //准备
            Product product = new Product { ProductID = 2, Name = "Test" };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID=1,Name="O1"},
                product,
                new Product { ProductID=3,Name="O3"},
            });
            ProductsController target = new ProductsController(mock.Object);
            //动作
            target.Delete(product.ProductID);
            //断言
            mock.Verify(m => m.DeleteProduct(product.ProductID));
        }
    }
}