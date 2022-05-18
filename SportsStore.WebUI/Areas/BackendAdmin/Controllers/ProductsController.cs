using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Concrete;
using SportsStore.Shared.Entities;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Areas.BackendAdmin.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private IProductRepository repository;

        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
        }

        // GET: BackendAdmin/Products
        public ViewResult Index()
        {
            return View(repository.Products);
        }

        // GET: BackendAdmin/Products/Details/5
        public ViewResult Details(int? id)
        {
            Product product = repository.Products.FirstOrDefault(x => x.ProductID == id);
            return View(product);
        }

        // GET: BackendAdmin/Products/Create
        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        // GET: BackendAdmin/Products/Edit/5
        public ViewResult Edit(int productID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            return View(product);
        }

        // POST: BackendAdmin/Products/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性。有关
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Description,Price,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(product).State = EntityState.Modified;
                repository.SaveProduct(product);
                TempData["message"] = $"{product.Name}已更新";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // POST: BackendAdmin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int productID)
        {
            Product deleteProduct = repository.DeleteProduct(productID);
            if (deleteProduct != null)
            {
                TempData["message"] = $"{deleteProduct.Name}已经被删除";
            }
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
