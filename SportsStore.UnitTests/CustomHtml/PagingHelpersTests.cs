/*****************************************************************************
*项目名称:SportsStore.UnitTests.CustomHtml
*项目描述:
*类 名 称:PagingHelpersTests
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 9:21:36
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Shared.ViewModel;

namespace SportsStore.Shared.CustomHtml.Tests
{
    [TestClass()]
    public class PagingHelpersTests
    {
        [TestMethod()]
        public void Can_Grenerate_PageLinks()
        {
            //准备1 - 定义Html辅助器,用于运用扩展方法
            HtmlHelper myHelper = null;
            //准备2 - 创建PagingInfo 数据
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            //准备3 - 创建lamdba委托
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //动作
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //断言
            var assertResult = @"<a class=""btn btn-default"" href=""Page1"">1</a>" +
                               @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
                               @"<a class=""btn btn-default"" href=""Page3"">3</a>";
            Assert.AreEqual(assertResult, result.ToString());
        }
    }
}