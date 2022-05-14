/*****************************************************************************
*项目名称:SportsStore.Domain.Concrete
*项目描述:
*类 名 称:ProductReporsitory
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/13 21:49:58
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Collections.Generic;

namespace SportsStore.Domain.Concrete
{
    public class ProductRepository : IProductRepository
    {
        private readonly EFDbContext dbContext = new EFDbContext();
        public IEnumerable<Product> Products
        {
            get { return dbContext.Products; }
        }

    }
}
