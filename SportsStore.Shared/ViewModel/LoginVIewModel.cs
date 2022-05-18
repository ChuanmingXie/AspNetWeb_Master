/*****************************************************************************
*项目名称:SportsStore.Shared.ViewModel
*项目描述:
*类 名 称:LoginVIewModel
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/18 10:23:45
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Shared.ViewModel
{
    public class LoginVIewModel
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
