## 1. 使用数据库
### i. 上下文类与连接字符串
#### A. 创建上下文类
    public class DbProductContext : DbContext
    {
        public DbProductContext():base("ProductDB") { }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //EF6与EFCore不同，不支持Column特性的使用,故针对decimal的精度通过重写OnModelCreating设置
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(18, 4));
        }
    }
#### B. 添加数据库连接字符串
	<connectionStrings>
		<add name="ProductDB" connectionString="Server=172.1**.***.***;Database=ProductContext_mvc;User ID=sa;Password=********;Trusted_Connection=false;MultipleActiveResultSets=true" providerName="System.Data.SqlClient"/>
	</connectionStrings>

### ii. EF6 CodeFirst 迁移与种子数据初始化
#### A. 迁移命令
    分层项目的数据迁移需要指定具体在那个子项目上进行迁移
    Enable-Migrations -ProjectName SportsStore.Domain -StartUpProjectName SportsStore.WebUI -Verbose
#### B. 创建迁移与设置种子数据
    Add-Migration Initial -projectname SportsStore.Domain -StartUpProjectName SportsStore.WebUI -Verbose
    迁移命令执行后在生成的Migrations文件夹内Configuration类的seed方法中设置种子数据
    context.Products.AddOrUpdate(i => i.Name
    , new Product { Name = "Kayak", Description = "A boat for on Preson", Category = "Watersports", Price = 275.00M }
    , new Product { Name = "Lifejacket", Description = "A boat for on Preson", Category = "Watersports", Price = 48.95M }
    , new Product { Name = "Soccer Ball", Description = "A boat for on Preson", Category = "Soccer", Price = 19.50M }
    , new Product { Name = "Corner Flags", Description = "A boat for on Preson", Category = "Soccer", Price = 34.95M }
    , new Product { Name = "Stadium", Description = "A boat for on Preson", Category = "Soccer", Price = 79500.00M }
    , new Product { Name = "Thinking Cap", Description = "A boat for on Preson", Category = "Chess", Price = 17.00M }
    , new Product { Name = "Unsteady Chair", Description = "A boat for on Preson", Category = "Chess", Price = 29.950M }
    , new Product { Name = "Human Chess Board", Description = "A boat for on Preson", Category = "Chess", Price = 75.00M }
    , new Product { Name = "Bling-Bling King", Description = "A boat for on Preson", Category = "Chess", Price = 1200.00M }
    );
#### C. 与更新数据库命令
    将种子函数中数据更新到数据库
    Update-database -projectname SportsStore.Domain -StartUpProjectName SportsStore.WebUI -Verbose

## 2. 分页功能
### i. 实现存储库接口与完成DI控制器
#### A. SportsStore.Domain项目Concrete文件夹下创建具体存储库ProductRepository
    public class ProductRepository : IProductRepository
    {
        private readonly DbProductContext context = new DbProductContext();
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
    }

#### B. Ninject已经完成了DI容器的构造
    在DI解析器中将模拟存储库的数据改为绑定具体存储库，删除MockRepository(),新增AddBindings();
    private void AddBindings()
    {
        kernel.Bind<IProductRepository>().To<ProductRepository>();
    }
### ii. 分页具体步骤-TDD红绿重构开发流程
#### A. 初始控制器逻辑
    初始版本的分页功能,控制器里有分页能力但没有分页按钮
    public class HomeController : Controller
    {
        private readonly IProductRepository repository;
        public int PageSize = 4;
        public HomeController(IProductRepository repository)
        {
            this.repository = repository;
        }
        // GET: Home
        public ViewResult IndexPageNoLinks(int page = 1)
        {
            return View(repository.Products
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                );
        }
    }
#### B. 测试分页逻辑
    针对以上分页功能进行测试
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
            IEnumerable<Product> result = (IEnumerable<Product>)controller.IndexNoPage(2).Model;

            //断言
            Product[] products = result.ToArray();
            Assert.IsTrue(products.Length == 3);
            Assert.AreEqual(products[0].Name, "P4");
            Assert.AreEqual(products[1].Name, "P5");
        }
    }
#### C. 自定义分页Html链接辅助器
##### a. 辅助器模型
[PagingInfo](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.Shared/ViewModel/PagingInfo.cs "分页辅助器模型")
##### b. 添加辅助器类
    针对分页特点自定义
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
[PagingHelpers](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.Shared/CustomHtml/PagingHelpers.cs "分页辅助器类")
##### c. 测试辅助器方法
[PagingHelpersTests](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.UnitTests/CustomHtml/PagingHelpersTests.cs "分页辅助器测试")
##### d. 配置辅助器方法
    在修改WebUI项目里Views文件夹内的web.config
    <pages pageBaseType="System.Web.Mvc.WebViewPage">
        <namespaces>
            <add namespace="System.Web.Mvc" />
            <add namespace="System.Web.Mvc.Ajax" />
            <add namespace="System.Web.Mvc.Html" />
            <add namespace="System.Web.Routing" />
            <add namespace="SportsStore.WebUI" />
            <add namespace="SportsStore.Shared.CustomHtml"/>
        </namespaces>
    </pages>
#### D. 新增分页数据模型
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
#### E. 改进控制器逻辑
    public class HomeController : Controller
    {
        private readonly IProductRepository repository;
        public int PageSize = 4;
        public HomeController(IProductRepository repository)
        {
            this.repository = repository;
        }
        public ViewResult Index(int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                                        .OrderBy(p => p.ProductID)
                                        .Skip((page - 1) * PageSize)
                                        .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            };
            return View(model);
        }
    }
#### F. 测试分页逻辑
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
        ProductsListViewModel result = (ProductsListViewModel)controller.Index(2).Model;

        //断言
        PagingInfo pagingInfo = result.PagingInfo;
        Assert.AreEqual(pagingInfo.CurrentPage, 2);
        Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
        Assert.AreEqual(pagingInfo.TotalItems, 5);
        Assert.AreEqual(pagingInfo.TotalPages, 2);
    }
[HomeControllerTests](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.UnitTests/Controllers/HomeControllerTests.cs "分页测试")
#### G. 视图渲染
    @model SportsStore.Shared.ViewModel.ProductsListViewModel
    ...
    @foreach (var p in Model.Products)
    {
        <div class="well">
            <h3>
                <strong>@Model.Name</strong>
                <span class="pull-right label label-primary">@Model.Price.ToString("c")</span>
            </h3>
            <span class="lead">@Model.Description</span>
        </div>
    }
    <div class="pager">
        @Html.PageLinks(Model.PagingInfo, x => Url.Action("Index", new { page = x }))
    </div>
### iii. 优化
#### A. 改进URL路由
    与默认的查询格式的URL
    https://localhost:44306/?Page=1
    不同 在设计上分页的连接URL格式为
    https://localhost:44306/Page1
    所以此处图书作者演示如何添加新的路由格式
    routes.MapRoute(
        name: null,
        url: "Page{page}",
        defaults: new { Controller = "Home", action = "Index" }
    );
#### B. 使用分部视图
[Index](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.WebUI/Views/Home/Index.cshtml "调用分部视图")<br>
[ProductSummary](https://github.com/ChuanmingXie/AspNetWeb_Master/blob/master/SportsStore.WebUI/Views/Home/ProductSummary.cshtml "分部视图")

## 3. 导航功能
### i. 过滤产品列表
### ii. 改进URL路由
    routes.MapRoute(null, "", new { controller = "Home", action = "Index", category = (string)null, page = 1 });
    routes.MapRoute(null, "Page{page}", new { controller = "Home", action = "Index", category = (string)null }, new { page = @"\d+" });
    routes.MapRoute(null, "category", new { controller = "Home", action = "Index", page = 1 });
    routes.MapRoute(null, "{category}/Page{page}", new { controller = "Home", action = "Index" }, new { page = @"\d+" });

### iii. 建立菜单
    在进行菜单及案例的测试时会出现如下的报错
    缺少编译器要求的成员“Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create”
    此时需要执行 添加引用 -> 程序集 -> 框架 Miscrosoft.CSharp 进行引用的添加
### vi. 修正页面计数

