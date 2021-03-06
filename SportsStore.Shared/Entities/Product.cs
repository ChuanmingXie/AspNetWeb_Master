/*****************************************************************************
*项目名称:SportsStore.Domain.Entities
*项目描述:
*类 名 称:Product
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/14 15:53:32
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SportsStore.Shared.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue =false)]
        public int ProductID { get; set; }

        [Display(Name = "名称")]
        [Required(ErrorMessage ="请输入产品名称")]
        [StringLength(512)]
        public string Name { get; set; }

        [Display(Name = "描述")]
        [StringLength(int.MaxValue)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "价格")]
        [Required,Range(0.01,double.MaxValue, ErrorMessage ="请输入有效的价格")]
        [DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18,2)")] EF Core才支持
        public decimal Price { get; set; }

        [Display(Name = "分类")]
        [Required(ErrorMessage ="请指定产品分类")]
        [StringLength(60)]
        public string Category { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }

    }
}
