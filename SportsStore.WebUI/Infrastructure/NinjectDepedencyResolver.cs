/*****************************************************************************
*项目名称:SportsStore.WebUI.Infrastructure
*项目描述:
*类 名 称:NinjectDepedencyResolver
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/14 10:51:44
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    /// <summary>
    /// 初始化基础结构的新实例 .NinjectDependencyResolver 类.
    /// </summary>
    public class NinjectDepedencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDepedencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            
            kernel.Bind<IProductRepository>().To<ProductRepository>();
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