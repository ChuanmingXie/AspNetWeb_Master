/*****************************************************************************
*项目名称:SportsStore.Shared.Services
*项目描述:
*类 名 称:EmailOutlookProcessor
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/16 11:32:47
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Shared.DataInterface;
using SportsStore.Shared.Entities;
using Microsoft.Office.Interop.Outlook;
using SportsStore.Shared.Common;

namespace SportsStore.Shared.Services
{
    public class EmailOutlookProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOutlookProcessor(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void ProcessOrder(ShopCart shopCart, ShoppingDetails details)
        {
            Application outlookEmial = new Application();
            MailItem mailItem = (MailItem)outlookEmial.CreateItem(OlItemType.olMailItem);
            mailItem.To = "468448790@qq.com";
            mailItem.Subject = "商品清单";
            mailItem.BodyFormat = OlBodyFormat.olFormatHTML;
            string content = "附件为 1数据，请查阅，谢谢!";
            content += "各收件人,<br/><br/>请重点关注以下内容";
            mailItem.HTMLBody = content;
            mailItem.Attachments.Add(@"d:\test.rar");
            ((_MailItem)mailItem).Send();
            mailItem=null;
            outlookEmial = null;
        }
    }
}
