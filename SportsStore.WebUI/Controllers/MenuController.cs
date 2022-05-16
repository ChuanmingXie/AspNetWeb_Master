using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IProductRepository repository;

        public MenuController(IProductRepository repository)
        {
            this.repository = repository;
        }

        // GET: MenuNav
        public PartialViewResult Index(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            //string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";
            //return PartialView(viewName, categories);
            return PartialView("IndexMenuFlex", categories);
        }
    }
}