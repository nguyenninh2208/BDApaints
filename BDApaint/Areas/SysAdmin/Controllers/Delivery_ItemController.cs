using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Delivery_ItemController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        // GET: SysAdmin/Delivery_Item
        public ActionResult Index()
        {
            var x = Entities.DELIVERY_ITEM.ToList();
            return View(x);
        }
        public int CreateID()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.DELIVERY_ITEM.Find(id) != null)
            {
                return CreateID();
            }
            if (id < 1)
                return CreateID();
            return id;
        }

        public ActionResult create()
        {
            var lst = Entities.DELIVERY_TYPE.ToList();
            ViewBag.type = new SelectList(lst, "DELIVERY_TYPE_ID", "DELIVERY_TYPE_NAME");
            var com = Entities.DELIVERies.ToList();
            ViewBag.company = new SelectList(com, "DELIVERY_ID", "DELIVERY_COMPANY_NAME");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult create(DELIVERY_ITEM d)
        {
            int rs = 0;
            DELIVERY_ITEM dc = new DELIVERY_ITEM();
            dc.DELIVERY_ITEM_ID = CreateID();
            dc.BOTH_NAME = d.BOTH_NAME;
            dc.DELIVERY_ID= d.DELIVERY_ID;
            dc.DELIVERY_TYPE_ID = d.DELIVERY_TYPE_ID;
            dc.ESTIMATED_DATE_DELIVERY = d.ESTIMATED_DATE_DELIVERY;
            dc.DELIVERY_ITEM_COST = d.DELIVERY_ITEM_COST;
            Entities.DELIVERY_ITEM.Add(dc);
            int row = Entities.SaveChanges();
            if (row >= 1)
            {
                rs = 1;
                setAlert("Thêm thành công", "success");
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                setAlert("Thêm không thành công", "error");
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult edit(int id)
        {
            var lst = Entities.DELIVERY_TYPE.ToList();
            ViewBag.type = new SelectList(lst, "DELIVERY_TYPE_ID", "DELIVERY_TYPE_NAME");
            var com = Entities.DELIVERies.ToList();
            ViewBag.company = new SelectList(com, "DELIVERY_ID", "DELIVERY_COMPANY_NAME");
            DELIVERY_ITEM dc = Entities.DELIVERY_ITEM.Find(id);
            return View(dc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit(DELIVERY_ITEM dc)
        {
            DELIVERY_ITEM d = Entities.DELIVERY_ITEM.Find(dc.DELIVERY_ITEM_ID);
            d.BOTH_NAME = dc.BOTH_NAME;
            d.DELIVERY_ID = dc.DELIVERY_ID;
            d.DELIVERY_TYPE_ID = dc.DELIVERY_TYPE_ID;
            d.DELIVERY_ITEM_COST = dc.DELIVERY_ITEM_COST;
            d.ESTIMATED_DATE_DELIVERY = dc.ESTIMATED_DATE_DELIVERY;
            int row = Entities.SaveChanges();
            if (row >= 1)
            {
                setAlert("Thêm thành công", "success");
                return Json(row, JsonRequestBehavior.AllowGet);
            }
            else
            {
                row = 0;
                setAlert("Cập nhật không thành công", "error");
                return Json(row, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult delete(int id)
        {
            int rs = 0;
            DELIVERY_ITEM d = Entities.DELIVERY_ITEM.Find(id);
            var model = Entities.CARTs.Where(x => x.DELIVERY_ITEM_ID == d.DELIVERY_ITEM_ID).ToList();
            if (model.Count != 0)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Entities.DELIVERY_ITEM.Remove(d);
                int row = Entities.SaveChanges();
                rs = row;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
    }
}