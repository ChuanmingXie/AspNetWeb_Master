using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.Knowleadge.Areas.RoutesHighAttribute.Controllers
{
    public class HomeController : Controller
    {
        // GET: RoutesHighAttribute/Home
        public ActionResult Index()
        {
            return View();
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