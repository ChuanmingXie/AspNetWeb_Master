using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.Knowleadge.Areas.RoutesHighAttribute.Controllers
{
    //[RouteArea("RoutesHighAttribute")]
    //[RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        // GET: RoutesHighAttribute/Admin
        //[Route("TestAction")]
        public ActionResult Index()
        {
            ViewBag.Controller = "Admin";
            ViewBag.Action = "Index";
            return View("ActionName");
        }


        public ActionResult CustomVariable(string id)
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "CustomVariable";
            ViewBag.CustomVariable = id;
            return View("ActionName");
        }
    }
}