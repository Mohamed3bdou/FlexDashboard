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

            Configuration webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");

            if (userName == webConfigApp.AppSettings.Settings["UserName"].Value && password == webConfigApp.AppSettings.Settings["Password"].Value)
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
            return RedirectToAction("Login", "Home");
        }
        #endregion
    }
}