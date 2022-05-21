using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.Knowleadge.Controllers
{
    [RoutePrefix("Users")]
    public class CustomerController : Controller
    {
        // GET: Home
        [Route("Test")]
        public ActionResult Index()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "Index";
            return View("ActionName");
        }

        [Route("Users/Add/{user}/{int:id}")]
        public string Create(string user, int id)
        {
            return $"创建动作 - 用户:{user},ID:{id}";
        }

        [Route("Users/Add/{user}/{passowrd}")]
        public string ChangePass(string user, string password)
        {
            return $"改变密码动作 - User:{user},Passowrd:{password}";
        }

        [Route("Users/Add/{user}/{passowrd:alpha:length(6)}")]
        public string ChangePassWord(string user, string password)
        {
            return $"改变密码动作 - User:{user},Passowrd:{password}";
        }

        public ActionResult List()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "List";
            return View("ActionName");
        }
    }
}