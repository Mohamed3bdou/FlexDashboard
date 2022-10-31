using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        DashboardDBEntities db = new DashboardDBEntities();
        #region Index
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Base");
            }
            else
            {
                ViewBag.CompId = new SelectList(db.Companies.ToList(), "s_dbName", "s_CompNameEng");
                ViewBag.n_customer_id = new SelectList(db.ar_customers.Where(x => x.s_dbname == "Xceer_14_2022").ToList(), "n_customer_id", "s_customer_name");
                ViewBag.n_salesman_id = new SelectList(db.ar_salesmen.Where(x => x.s_dbname == "Xceer_14_2022").ToList(), "n_salesman_id", "s_salesman_name");

                ExpensesChart("Xceer_14_2022", null, null);
                //SalerChart("Xceer_14_2022", null, null, null, null);

                return View();
            }
        }
        #endregion

        #region Customers & Salers Lists
        public ActionResult ReloadCustomersList(string dataBase)
        {
            var list = db.ar_customers.Where(x => x.s_dbname == dataBase).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReloadSalersList(string dataBase)
        {
            var list = db.ar_salesmen.Where(x => x.s_dbname == dataBase).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Balances
        public ActionResult Balance(string dataBase)
        {
            var bankList = db.Balances.Where(x => x.s_DbName == dataBase).Select(x => new { x.s_TreasureName, x.s_Value }).ToList();
            return Json(bankList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Expenses Chart
        public ActionResult ExpensesChart(string dataBase, string from, string to)
        {
            var list = db.dx_expenses_PROC(
                dataBase == null ? null : dataBase,
                from = string.IsNullOrEmpty(from) ? null : Convert.ToDateTime(from).ToString("yyyy/MM/dd"),
                to = string.IsNullOrEmpty(to) ? null : Convert.ToDateTime(to).ToString("yyyy/MM/dd")).OrderBy(x => x.month).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Saler Chart
        public ActionResult SalerChart(string dataBase, string from, string to, string customer, string saler)
        {
            var list = db.dx_sales_proc(
                dataBase == null ? null : dataBase,
                from = string.IsNullOrEmpty(from) ? null : Convert.ToDateTime(from).ToString("yyyy/MM/dd"),
                to = string.IsNullOrEmpty(to) ? null : Convert.ToDateTime(to).ToString("yyyy/MM/dd"),
                customer = string.IsNullOrEmpty(customer) ? null : customer,
                saler = string.IsNullOrEmpty(saler) ? null : saler).OrderBy(x => x.nMonth).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}