using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.Knowleadge.Areas.RoutesHighAttribute.Controllers
{
    public class LegacyController : Controller
    {
        // GET: RoutesHighAttribute/Legacy
        public ActionResult GetLegacyURL(string legacyURL)
        {
            return View((object)legacyURL);
        }
    }
}