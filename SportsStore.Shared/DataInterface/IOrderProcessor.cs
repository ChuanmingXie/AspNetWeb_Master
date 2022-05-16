/*****************************************************************************
*项目名称:SportsStore.Shared.DataInterface
*项目描述:
*类 名 称:IOrderProcessor
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/16 9:40:08
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using SportsStore.Shared.Entities;

namespace SportsStore.Shared.DataInterface
{
    public interface IOrderProcessor
    {
        void ProcessOrder(ShopCart shopCart, ShoppingDetails details);
    }
}
