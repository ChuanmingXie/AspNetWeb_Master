using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Shared.DataInterface;
using SportsStore.Shared.ViewModel;

namespace SportsStore.WebUI.Areas.BackendAdmin.Controllers
{
    public class AccountController : Controller
    {
        private IAuthProvider authProvider;

        public AccountController(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        // GET: BackendAdmin/Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVIewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Products"));
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码错误");
                }
            }
            return View();
        }
    }
}