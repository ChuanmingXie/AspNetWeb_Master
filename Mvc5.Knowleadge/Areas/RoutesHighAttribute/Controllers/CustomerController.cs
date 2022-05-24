using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.Knowleadge.Areas.RoutesHighAttribute.Controllers
{
    [RouteArea("RoutesHighAttribute")]
    [RoutePrefix("User")]
    public class CustomerController : Controller
    {
        // GET: RoutesHighAttribute/Customer
        [Route("Test")]
        public ActionResult Index()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "Index";
            return View("ActionName");
        }

        public ActionResult List()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "List";
            return View("ActionName");
        }

    }
}