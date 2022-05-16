/*****************************************************************************
*项目名称:SportsStore.Shared.Entities
*项目描述:
*类 名 称:EmailSettings
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/16 9:30:54
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

namespace SportsStore.Shared.Common
{
    public class EmailSettings
    {
        public string MailToAdress = "287713607@qq.com";
        public string MailFromAdress = "468448790@qq.com";
        //public string MailFromAdress = "chuanmingxie@outlook.com";
        public bool UseSsl = true;
        public string Username = "468448790@qq.com";
        //public string Username = "chuanmingxie@outlook.com";
        //public string Password = "xyyxyb89";
        public string Password = "zukkkswurqznbiae";
        public string ServerName = "smtp.qq.com";
        public int ServerPort = 25;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\sports_store_emails";

        //POP3/SMTP服务  zukkkswurqznbiae

        //IMAP/SMTP服务 xruvaibcfcdvcacd

        //Exchange服务 sxepylvgzmcmbgdh
    }
}
