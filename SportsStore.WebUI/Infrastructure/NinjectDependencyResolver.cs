/*****************************************************************************
*项目名称:SportsStore.WebUI.Infrastructure
*项目描述:
*类 名 称:NinjectDependencyResolver
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/14 16:21:23
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Shared.Entities;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            //MockRepository();
            kernel.Bind<IProductRepository>().To<ProductRepository>();
        }

        private void MockRepository()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(new List<Product>
            {
                new Product {Name="篮球",Price=34},
                new Product {Name="足球",Price=134},
                new Product {Name="跑鞋",Price=94},
            });
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}