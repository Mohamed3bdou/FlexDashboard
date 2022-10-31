using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace Dashboard.Controllers
{
    public class BaseController : Controller
    {
        DashboardDBEntities db = new DashboardDBEntities();

        #region Login
        [ActionName("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult validateLogin(FormCollection obj)
        {
            string userName = obj["userName"];
            string password = obj["password"];
            var user = db.TbUsers.Where(x => x.s_userName.Equals(userName) && x.s_password.Equals(password)).FirstOrDefault();

            if (user != null)
            {
                Session["UserName"] = obj["userName"];
                ViewBag.result = "Successful";

                return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.result = "Incorrect Password";

            return View();
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Base");
        }
        #endregion
    }
}