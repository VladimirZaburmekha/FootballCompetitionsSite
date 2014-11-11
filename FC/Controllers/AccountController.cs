using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FC.Models;
using FC.Services;
//using System.Web.Helpers.Crypto;

namespace FC.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
      /*  public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        */
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Форма заповнена не вірно, спробуйте ще раз.";
                return View();
            }
            var currentAdmin =
                new BaseService<Admin>(new FootballCompetitionsEntities()).Find(
                    a => a.AdminLogin == admin.AdminLogin && a.Password == admin.Password);
            if (currentAdmin!=null)
                FormsAuthentication.RedirectFromLoginPage(admin.AdminLogin, true);

            ViewBag.Error = "пароль чи логін не вірний.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("ShowAllMatches", "Matches");
        }
    }
}
