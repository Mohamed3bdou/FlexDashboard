using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    public class BranchesController : BaseController
    {
        DashboardDBEntities db = new DashboardDBEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Base");
            }
            else
            {
                ViewBag.CompId = new SelectList(db.Companies.ToList(), "s_dbName", "s_CompNameEng");
                ViewBag.n_branch_id = new SelectList(db.Branches.Where(x => x.s_dbname == "Xceer_14_2022").ToList(), "n_branch_id", "s_branch_name");
                ViewBag.n_customer_id = new SelectList(db.ar_customers.ToList(), "n_customer_id", "s_customer_name");
                ViewBag.n_salesman_id = new SelectList(db.ar_salesmen.ToList(), "n_salesman_id", "s_salesman_name");
                GetBranches(null);
                return View();
            }
        }

        #region Get Branches
        public ActionResult GetBranches(string dataBase)
        {
            var list = db.Branches.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase)).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region Get Customers Balance Data
        [HttpGet]
        public ActionResult GetCustomersBalances(string dataBase, int branch)
        {
            var list = db.BranchCustomerBalances.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase) && x.n_branch_id == (branch == 0 ? 1 : branch)).Select(x => new { x.n_branch_id, x.s_branch_name, x.n_customer_balance }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Treasures Balance Data
        [HttpGet]
        public ActionResult GettreasuresBalance(string dataBase, int branch)
        {
            var list = db.BranchesTreasureBalances.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase) && x.n_branch_no == (branch == 0 ? 1 : branch)).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Expenses Charts
        public ActionResult ExpensesChart(string dataBase, string from, string to, int branchId)
        {
            var list = db.dx_expenses_proc_Branch(
                dataBase == null ? null : dataBase,

                from = string.IsNullOrEmpty(from) ? null : Convert.ToDateTime(from).ToString("yyyy/MM/dd"),
                to = string.IsNullOrEmpty(to) ? null : Convert.ToDateTime(to).ToString("yyyy/MM/dd"),
                branchId == 0 ? 1 : branchId).OrderBy(x => x.month).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Saler Chart
        public ActionResult SalerChart(string dataBase, int branchId, string from, string to, string customer, string saler)
        {
            var list = db.dx_sales_proc_Branch(
                dataBase == null ? null : dataBase,
                branchId == 0 ? 1 : branchId,
                from = string.IsNullOrEmpty(from) ? null : Convert.ToDateTime(from).ToString("yyyy/MM/dd"),
                to = string.IsNullOrEmpty(to) ? null : Convert.ToDateTime(to).ToString("yyyy/MM/dd"),
                customer = string.IsNullOrEmpty(customer) ? null : customer,
                saler = string.IsNullOrEmpty(saler) ? null : saler).OrderBy(x => x.nMonth).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Customers & Salers Lists
        public ActionResult ReloadCustomersList(string dataBase)
        {
            var list = db.ar_customers.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase)).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReloadSalersList(string dataBase)
        {
            var list = db.ar_salesmen.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase)).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}