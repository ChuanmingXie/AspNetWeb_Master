using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Shared.Entities;
/*****************************************************************************
*项目名称:SportsStore.UnitTests.Entities
*项目描述:
*类 名 称:ShopCartTests
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 17:38:42
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

namespace SportsStore.Shared.Entities.Tests
{
    [TestClass()]
    public class ShopCartTests
    {
        [TestMethod()]
        public void Can_Add_New_Lines()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            ShopCart target = new ShopCart();
            //动作
            target.AddItemProduct(p1, 1);
            target.AddItemProduct(p2, 1);
            CartLine[] result = target.Lines.ToArray();
            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
        }

        [TestMethod()]
        public void Add_Quantity_For_Existing_Lines()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            ShopCart target = new ShopCart();
            //动作
            target.AddItemProduct(p1, 1);
            target.AddItemProduct(p2, 1);
            target.AddItemProduct(p1, 10);
            CartLine[] result = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();
            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 11);
            Assert.AreEqual(result[1].Quantity, 1);
        }

        [TestMethod()]
        public void Can_Remove_Line()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            ShopCart target = new ShopCart();
            //动作
            target.AddItemProduct(p1, 1);
            target.AddItemProduct(p2, 3);
            target.AddItemProduct(p3, 5);
            target.AddItemProduct(p2, 1);
            target.RemoveLine(p2);
            var result = target.Lines.Where(c => c.Product == p2).Count();
            //断言
            Assert.AreEqual(result, 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod()]
        public void Compute_Total_Price()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            ShopCart target = new ShopCart();
            //动作
            target.AddItemProduct(p1, 1);
            target.AddItemProduct(p1, 3);
            target.AddItemProduct(p2, 1);
            decimal result = target.ComputeTotalValue();
            //断言
            Assert.AreEqual(result, 450M);
        }

        [TestMethod()]
        public void Can_Clear_ShopCart()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            ShopCart target = new ShopCart();
            target.AddItemProduct(p1, 1);
            target.AddItemProduct(p2, 4);
            target.AddItemProduct(p1, 1);
            //动作
            target.Clear();
            //断言
            Assert.AreEqual(target.Lines.Count, 0);
        }
    }
}