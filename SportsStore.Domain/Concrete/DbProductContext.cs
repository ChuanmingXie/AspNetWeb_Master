/*****************************************************************************
*项目名称:SportsStore.Domain.Concrete
*项目描述:
*类 名 称:DbContext
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/14 15:56:38
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System.Data.Entity;
using SportsStore.Shared.Entities;

namespace SportsStore.Domain.Concrete
{
    public class DbProductContext : DbContext
    {
        public DbProductContext():base("ProductDB")
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(18, 4));
        }
    }
}
