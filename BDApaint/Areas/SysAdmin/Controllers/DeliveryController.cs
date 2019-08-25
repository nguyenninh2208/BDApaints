using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class DeliveryController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        // GET: SysAdmin/DELIVERies
        public ActionResult Index()
        {
            var model = Entities.DELIVERies.ToList();
            return View(model);
        }
        public int CreateID()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.DELIVERies.Find(id) != null)
            {
                return CreateID();
            }
            if (id < 1)
                return CreateID();
            return id;
        }

        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult create(DELIVERY d)
        {
            int rs = 0;
            DELIVERY dc = new DELIVERY();
            dc.DELIVERY_ID = CreateID();
            dc.DELIVERY_COMPANY_NAME = d.DELIVERY_COMPANY_NAME;
            dc.DELIVERY_PHONE = d.DELIVERY_PHONE;
            dc.DELIVERY_ADDRESS = d.DELIVERY_ADDRESS;
            Entities.DELIVERies.Add(dc);
            int row = Entities.SaveChanges();
            if(row >=1)
            {
                rs = 1;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult edit(int id)
        {
            DELIVERY dc = Entities.DELIVERies.Find(id);
            return View(dc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit(DELIVERY dc)
        {
            DELIVERY d = Entities.DELIVERies.Find(dc.DELIVERY_ID);
            d.DELIVERY_COMPANY_NAME = dc.DELIVERY_COMPANY_NAME;
            d.DELIVERY_PHONE = dc.DELIVERY_PHONE;
            d.DELIVERY_ADDRESS = dc.DELIVERY_ADDRESS;
            int row = Entities.SaveChanges();
            if(row >= 1)
            {
                return Json(row, JsonRequestBehavior.AllowGet);
            }
            else
            {
                row = 0;
                return Json(row, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult delete(int id)
        {
            int rs = 0;
            DELIVERY d = Entities.DELIVERies.Find(id);
            var model = Entities.DELIVERY_ITEM.Where(x => x.DELIVERY_ID == d.DELIVERY_ID).ToList();
            if(model.Count != 0)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Entities.DELIVERies.Remove(d);
                int row = Entities.SaveChanges();
                rs = row;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
    }
}