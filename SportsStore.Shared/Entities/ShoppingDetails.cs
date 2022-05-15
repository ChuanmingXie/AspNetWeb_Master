/*****************************************************************************
*项目名称:SportsStore.Shared.Entities
*项目描述:
*类 名 称:ShoppingDetails
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 22:15:27
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

namespace SportsStore.Shared.Entities
{
    public class ShoppingDetails
    {
        [Required(ErrorMessage = "请输入姓名")]
        [Display(Name = "客户姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入手机号码")]
        [Display(Name = "手机号码")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "请输入一个有效地址")]
        [Display(Name="地址1")]
        public string Line1 { get; set; }

        [Display(Name="地址2")]
        public string Community { get; set; }

        [Display(Name="地址3")]
        public string BuldingRoom { get; set; }

        [Required(ErrorMessage = "请输入所属市区")]
        [Display(Name = "市区")]
        public string City { get; set; }

        [Required(ErrorMessage = "请输入所属省份")]
        [Display(Name = "省份")]
        public string Province { get; set; }
        public string Zip { get; set; }

        [Required(ErrorMessage = "请输入所属国家")]
        [Display(Name = "客户姓名")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }

    }
}
