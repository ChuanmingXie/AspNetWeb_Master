/*****************************************************************************
*项目名称:SportsStore.Domain.Abstract
*项目描述:
*类 名 称:IProductRepository
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/14 15:53:04
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using SportsStore.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
    }
}
