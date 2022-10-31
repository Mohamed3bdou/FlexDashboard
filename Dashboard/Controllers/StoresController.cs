using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    public class StoresController : BaseController
    {
        DashboardDBEntities db = new DashboardDBEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Login", "Base");
            else
            {
                ViewBag.CompId = new SelectList(db.Companies.ToList(), "s_dbName", "s_CompNameEng");

                return View();
            }
        }

        #region GetStoresValues
        [HttpGet]
        public ActionResult GetStoresValues(string dataBase)
        {
            var list = db.StoreValues.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase)).Select(x => new { x.n_store_id, x.s_store_name, x.n_store_value }).OrderByDescending(x => x.n_store_id).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetGroupSales
        [HttpGet]
        public ActionResult GetGroupSales(string dataBase)
        {
            var list = db.SalesByGroups.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase)).Select(x => new { x.s_GroupItem_name, x.n_value }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetGroupQuantitySales
        [HttpGet]
        public ActionResult GetGroupQuantitySales(string dataBase)
        {
            var list = db.SalesByGroupQuantities.Where(x => x.s_dbname == (string.IsNullOrEmpty(dataBase) == true ? "Xceer_14_2022" : dataBase)).Select(x => new { x.s_GroupItem_name, x.n_value }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}