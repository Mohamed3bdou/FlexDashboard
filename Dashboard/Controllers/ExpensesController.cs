using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    public class ExpensesController : BaseController
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
                PieChart(null, 0);
                return View();
            }
        }

        #region PieChart
        public ActionResult PieChart(string dataBase, int month)
        {
            var list = db.dx_monthly_expenses(dataBase == null ? null : dataBase, month == 0 ? 1 : month).Select(x => new { x.s_desc, x.CurrentMonth, x.PreviousMonth,  }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Monthes
        public ActionResult GetMonthes()
        {
            List<Monthes> monthes = new List<Monthes>()
            {
                new Monthes{ID = 1,  Name  = "يناير"  },
                new Monthes{ID = 2,  Name  = "فبراير" },
                new Monthes{ID = 3,  Name  = "مارس"   },
                new Monthes{ID = 4,  Name  = "ابريل"  },
                new Monthes{ID = 5,  Name  = "مايو"   },
                new Monthes{ID = 6,  Name  = "يونيو"  },
                new Monthes{ID = 7,  Name  = "يوليو"  },
                new Monthes{ID = 8,  Name  = "اغسطس"  },
                new Monthes{ID = 9,  Name  = "سبتمبر" },
                new Monthes{ID = 10, Name  = "اكتوبر" },
                new Monthes{ID = 11, Name  = "نوفمبر" },
                new Monthes{ID = 12, Name  = "ديسمبر" },
            };
            return Json(monthes, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}