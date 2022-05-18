## 1. 系统后台数据管理
    CRUD：创建、读取、更新和删除
### i. 创建后台区域与布局页设置
严重性	代码	说明	项目	文件	行	禁止显示状态
错误	CS1929	“IEnumerable<Product>”不包含“ToListAsync”的定义，并且最佳扩展方法重载“QueryableExtensions.ToListAsync(IQueryable)”需要类型为“IQueryable”的接收器	SportsStore.WebUI	D:\CodePractice\AspNetWeb_Master\SportsStore.WebUI\Areas\BackendAdmin\Controllers\ProductsController.cs	30	活动

### ii.index列表功能 
    控制器index列表动作
#### A. index列表控制器动作
    System.InvalidOperationException: The source IQueryable doesn't implement IDbAsyncEnumerable<Product>. Only sources that implement IDbAsyncEnumerable can be used for Entity Framework asynchronous operations. For more details see
#### B. 测试控制器动作
#### C. 视图

### iii. Edit功能
#### A. Edit控制器Get进入视图动作
#### B. 测试控制器动作
#### C. 视图
##### a.模型辅助器
##### b.模型元数据
##### c.优化表单视图显示
    * 为辅助器生成的内容定义CSS样式
    * 向辅助器提供模板，使之自动生成所需要的包含样式的表单元素
    * 直接创建需要的表单元素，而不使用模型辅助器HtmlHelper
#### D. Edit控制器POST提交动作
#### E. 测试控制器动作
#### F. 客户端信息提示与验证
##### a. 显示确认消息
##### b. 添加模型验证
##### c. 客户端验证
    Install-package Microsoft.jQuery.Unobtrusive.Validation -version 
### vi. Add功能
#### A. Add控制器动作
#### B. 视图

### v. Delete功能
#### A. Delete控制器动作
#### B. 测试控制器动作
#### C. 删除操作视图
    Server Error in '/' Application.
    The required anti-forgery form field "__RequestVerificationToken" is not present.
    Description: An unhandled exception occurred during the execution of the current web request. Please review the stack trace for more information about the error and where it originated in the code.

    Exception Details: System.Web.Mvc.HttpAntiForgeryException: The required anti-forgery form field "__RequestVerificationToken" is not present.

    Source Error:

    An unhandled exception was generated during the execution of the current web request. Information regarding the origin and location of the exception can be identified using the exception stack trace below.


## 2. 用户登录验证
## 3. 商品图片数据管理

