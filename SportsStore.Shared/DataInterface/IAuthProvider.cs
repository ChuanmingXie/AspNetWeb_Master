﻿/*****************************************************************************
*项目名称:SportsStore.Shared.DataInterface
*项目描述:
*类 名 称:IAuthProvider
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/18 10:17:08
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

namespace SportsStore.Shared.DataInterface
{
    public interface IAuthProvider
    {
        bool Authenticate(string name, string password);
    }
}
