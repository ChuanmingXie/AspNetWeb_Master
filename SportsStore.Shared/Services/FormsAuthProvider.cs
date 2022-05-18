/*****************************************************************************
*项目名称:SportsStore.Shared.Services
*项目描述:
*类 名 称:FormsAuthProvider
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/18 10:18:17
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
using System.Web.Security;
using SportsStore.Shared.DataInterface;

namespace SportsStore.Shared.Services
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string userName, string password)
        {
#pragma warning disable CS0618 // 类型或成员已过时
            bool result = FormsAuthentication.Authenticate(userName, password);
#pragma warning restore CS0618 // 类型或成员已过时
            if (result)
                FormsAuthentication.SetAuthCookie(userName, false);
            return result;
        }
    }
}
