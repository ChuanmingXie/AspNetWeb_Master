/*****************************************************************************
*项目名称:SportsStore.Shared.ViewModel
*项目描述:
*类 名 称:PagingInfo
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 8:59:13
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

namespace SportsStore.Shared.ViewModel
{
    public class PagingInfo
    {
        private int totalItems;

        /// <summary>
        /// 条目总数
        /// </summary>
        public int TotalItems { get => totalItems; set => totalItems = value; }

        /// <summary>
        /// 每页条目数
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }
    }
}
