using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Import_Order_DetailController : BaseController
    {
        // GET: Import_Order_Detail
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getDetailByID(int id)
        {
            var import = Entities.IMPORT_ORDER.Where(x => x.ID == id).First();
            ViewBag.ID = import.ID;
            ViewBag.Phone = import.SUPPLIER.PHONE;
            ViewBag.PersonName = import.PERSON_IMPORT.NAME;
            ViewBag.Total = import.TOTAL;
            ViewBag.Date = import.DATE_IMPORT;
            ViewBag.Sup = import.SUPPLIER.NAME;
            ViewBag.Email = import.SUPPLIER.EMAIL;
            List<IMPORT_ORDER_DETAIL> lst = Entities.IMPORT_ORDER_DETAIL.Where(x => x.IMPORT_ID == id).ToList();
            return View("showListDetail",lst);
        }
        public ActionResult detail(int id)
        {
            var model = Entities.IMPORT_ORDER_DETAIL.Find(id);
            return View("detail",model);
        }
    }
}