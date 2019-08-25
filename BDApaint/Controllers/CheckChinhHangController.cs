using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Controllers
{
    public class CheckChinhHangController : Controller
    {
        //
        // GET: /CheckChinhHang/
        DATABASELVTN1Entities db = new DATABASELVTN1Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult resultSearchAuth(string searchString)
        {
            var lstProduct = db.PRODUCTs.Where(x => x.CODE_AUTH.Contains(searchString));
            ViewBag.searchString = searchString;
            ViewBag.count = lstProduct.Count();
            return View("resultSearchAuth", lstProduct.ToList());
        }
        [HttpPost]
        public ActionResult getSearch()
        {
            String searchString = Request["searchString"];
            return RedirectToAction("resultSearchAuth", new {searchString = searchString });
        }
    }
}



