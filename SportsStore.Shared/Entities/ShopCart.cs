/*****************************************************************************
*项目名称:SportsStore.Shared.Entities
*项目描述:
*类 名 称:ShopCart
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/15 17:28:41
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Shared.Entities
{
    public class ShopCart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public List<CartLine> Lines { get => lineCollection; }

        public void AddItemProduct(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
            => lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public decimal ComputeTotalValue()
            => lineCollection.Sum(d => d.Product.Price * d.Quantity);

        public void Clear()
            => lineCollection.Clear();
    }

    public class CartLine
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
