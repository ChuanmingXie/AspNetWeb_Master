# AspNetWeb_Master
	在.NET Framework4.8 平台和VS2019 IDE下重写《精通ASP.NET Mvc5》有关.NET Mvc5框架的相关TDD案例。
	统一研究一下书籍当中组件使用出现的问题
## 1. 创建构建<br>
### i. 创建新项目<br>
#### A. SportsStore.WebUI 项目的配置如下<br>
	模板选择： ASP.NET Web Application(.NET Framework)
	框架选择： .NET Framework4.8
	创建新的ASP.NET Web 应用程序选择： 空
	添加文件夹和核心引用选择： MVC
#### B. SportsStore.UnitTests 项目的配置如下
	模板选择： 单元测试项目(.NET Fremework)
	框架选择： .NET Framework4.8
#### C. SportsStore.Domian 项目配置如下
	模板选择： 类库(.NET Framework)
	框架选择： .NET Framework4.8
#### D. 依赖关系
	SportsStore.WebUI 依赖于Domain
	SportsStore.UnitTests 依赖于WebUI和Domain
### ii. 开发环境组件包安装：
#### A. 说明
	环境安装在.NET Framework4.8平台下有些命令是会引发错误的，可以参考书籍中相关版本但不可照搬照抄
	Ninject.Web.Common依赖于Ninject，所以两个包可以合并安装，只安装Ninject.Web.Common即可
	Microsoft.Aspnet.Mvc在.NET Framework4.8中使用版本5.2.7 所以 Domain和UnitTests安装5.2.7
	Ninject.MVC3安装的结果是Ninject.web.mvc暂时未发现其用处，笔者并未安装
#### B. 注意：
	书籍Ninject.Web.Common.cs自动加载为问题在网上有安装失败的现象，可全部复制下文C中的命令进行安装
	其中EntityFramework可不指定版本；Bootstrap以自己熟悉程度选择3/4/5/mdb5等版本；一可以选择Layui等前端框架
#### C. 命令：
	Install-package EntityFramework -projectname SportsStore.Domain
	Install-package Microsoft.Aspnet.Mvc -version 5.2.7 -projectname SportsStore.Domain

	Install-package EntityFramework -projectname SportsStore.WebUI
	Install-package Moq -version 4.18.0 -projectname SportsStore.WebUI
	Install-package Castle.Core -version 5.0.0 -projectname SportsStore.WebUI
	Install-package Ninject.Web.Common -version 3.0.0.7 -projectname SportsStore.WebUI
	Install-package Bootstrap -version 3.4.1 -projectname SportsStore.WebUI
	
	Install-package Moq -version 4.18.0 -projectname SportsStore.UnitTests
	Install-package Ninject.Web.Common -version 3.0.0.7 -projectname SportsStore.UnitTests
	Install-package Microsoft.Aspnet.Mvc -version 5.2.7 -projectname SportsStore.UnitTests

	暂时不安装
	* Install-package Ninject.MVC3 -version 3.0.0.6 -projectname SportsStore.WebUI
	* Install-package Ninject.MVC3 -version 3.0.0.6 -projectname SportsStore.UnitTests
## 2. 功能开发
### i. Ninject依赖注入容器
#### A. Ninject初始化
	在WebUI项目中创建 Infrastructure （基础设置）文件夹，
	在此文件夹中创建实现依赖注入解析接口IDependencyResolver的NinjectDependencyResolver类(初始化基础结构的新实例)
	因为项目内尚未建立模型，所以此文件需要在步骤B完成后进行补充
#### B. 域中创建模型与存储库并且完善Ninject解析类
	产品描述模型 product.cs
		具体实现参考:
[Product.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.Domain/Entities/Product.cs "Product.cs")
	抽象存储库 IProductRepository.cs 接口
[IProductRepository.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.Domain/Abstract/IProductRepository.cs "IProductRepository.cs")
	完善DI容器解析类
	具体实现参考:
[NinjectDepedencyResolver.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.WebUI/Infrastructure/NinjectDepedencyResolver.cs "NinjectDepedencyResolver.cs")
	注册DI容器解析
[NinjectWebCommon.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.WebUI/App_Start/NinjectWebCommon.cs "NinjectWebCommon.cs")
	只需要在自动添加的类里添加注册代码
	/// <summary>
	/// Load your modules or register your services here!
	/// </summary>
	/// <param name="kernel">The kernel.</param>
	private static void RegisterServices(IKernel kernel)
	{
		DependencyResolver.SetResolver(new Infrastructure.NinjectDependencyResolver(kernel));
	}
#### C. 模仿存储库数据
	在NinjectDepedencyResolver.cs文件中添加模仿方法，并在构造函数中调用
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
#### D. 视图数据渲染与呈现
	建立HomeController,通过构造函数注入抽象存储库，在Index动作方法中返回视图所要渲染的数据
	public class HomeController : Controller
	{
		private readonly IProductRepository repository;
		public HomeController(IProductRepository repository)
		{
			this.repository = repository;
		}
		// GET: Home
		public ActionResult Index()
		{
			return View(repository.Products);
		}
	}
	在视图中引用模型，并从模型中通过foreach剪辑出相关数据
	@using SportsStore.Domain.Entities
	@model IEnumerable<Product>
	...
	@foreach (var p in Model)
	{
		<h3>@p.Name</h3>
		@p.Description
		<h4>@p.Price.ToString("c")</h4>
	}