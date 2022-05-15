using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;
using SportsStore.Shared.ViewModel;

namespace SportsStore.WebUI.Controllers
{
    public class ShopCartController : Controller
    {
        private IProductRepository repository;

        public ShopCartController(IProductRepository repository)
        {
            this.repository = repository;
        }

        public RedirectToRouteResult AddToShopCart(int productID, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                GetShopCart().AddItemProduct(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFormShopCart(int productID, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                GetShopCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private ShopCart GetShopCart()
        {
            ShopCart shopCart = (ShopCart)Session["ShopCart"];
            if (shopCart == null)
            {
                shopCart = new ShopCart();
                Session["ShopCart"] = shopCart;
            }
            return shopCart;
        }

        // GET: ShopCart
        public ViewResult Index(string returnUrl)
        {
            return View(new ShopCartIndexViewModel
            {
                ShopCart = GetShopCart(),
                ReturnUrl = returnUrl
            });
        }
    }
}