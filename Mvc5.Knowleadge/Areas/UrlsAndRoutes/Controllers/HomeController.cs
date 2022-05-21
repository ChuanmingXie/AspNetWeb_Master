using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.Knowleadge.Areas.UrlsAndRoutes.Controllers
{
    public class HomeController : Controller
    {
        // GET: UrlsAndRoutes/Home
        public ActionResult Index()
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";
            ViewBag.CustomVariable = RouteData.Values["id"];
            return View("ActionName");
        }

        public ActionResult CustomVariable(string id)
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "CustomVariable";
            ViewBag.CustomVariable = id;
            return View("ActionName");
        }


        public ActionResult CustomVariableOptional(string id)
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "CustomVariableOptional";
            ViewBag.CustomVariable = id ?? "没有id值";
            return View("ActionName");
        }
    }
}