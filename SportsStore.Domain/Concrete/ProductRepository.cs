/*****************************************************************************
*项目名称:SportsStore.Domain.Concrete
*项目描述:
*类 名 称:ProductRepository
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/14 15:55:15
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System.Collections.Generic;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;

namespace SportsStore.Domain.Concrete
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbProductContext context = new DbProductContext();
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }
    }
}
