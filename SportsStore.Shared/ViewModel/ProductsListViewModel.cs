/*****************************************************************************
*项目名称:SportsStore.Shared.ViewModel
*项目描述:
*类 名 称:ProductsListViewModel
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 9:37:31
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
using SportsStore.Shared.Entities;

namespace SportsStore.Shared.ViewModel
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
