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
#### B. 注意：
	书籍Ninject.Web.Common.cs自动加载为问题在网上有安装失败的现象，可逐个复制C中的命令进行安装
	其中EntityFramework可不指定版本；Bootstrap以自己熟悉程度选择3/4/5/mdb5等版本；一可以选择Layui等前端框架
#### C. 命令：
	Install-package Ninject.Web.Common -version 3.0.0.7 -projectname SportsStore.WebUI
	Install-package Ninject.Web.Common -version 3.0.0.7 -projectname SportsStore.UnitTests
	Install-package Ninject.MVC3 -version 3.0.0.6 -projectname SportsStore.WebUI
	Install-package Ninject.MVC3 -version 3.0.0.6 -projectname SportsStore.UnitTests
	Install-package Moq -version 4.1.1309.1617 -projectname SportsStore.WebUI
	Install-package Moq -version 4.1.1309.1617 -projectname SportsStore.UnitTests
	Install-package Microsoft.Aspnet.Mvc -version 5.2.7 -projectname SportsStore.Domain
	Install-package Microsoft.Aspnet.Mvc -version 5.2.7 -projectname SportsStore.UnitTests
	Install-package EntityFramework -projectname SportsStore.WebUI
	Install-package EntityFramework -projectname SportsStore.Domain
	Install-package Bootstrap -version 3.4.1 -projectname SportsStore.WebUI
## 2. 功能开发
### i. Ninject依赖注入容器
#### A. Ninject初始化
	在WebUI项目中创建 Infrastructure （基础设置）文件夹，
	在此文件夹中创建实现依赖注入解析接口IDependencyResolver的NinjectDependencyResolver类(初始化基础结构的新实例)
	具体实现参考:
[NinjectDepedencyResolver.cs](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.WebUI/Infrastructure/NinjectDepedencyResolver.cs "NinjectDepedencyResolver.cs")
#### B. 域模型与存储库

#### C. 测试存储库

	

