using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Delivery_TypeController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        // GET: SysAdmin/Delivery_Type
        public ActionResult Index()
        {
            var model = Entities.DELIVERY_TYPE.ToList();
            return View(model);
        }

        public ActionResult create()
        {
            return View();
        }

        public int createID()
        {
                Random rand = new Random();
                int id = rand.Next(1, 1000);
                if (Entities.DELIVERY_TYPE.Find(id) != null)
                {
                    return createID();
                }
                if (id < 1)
                    return createID();
                return id;            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create(DELIVERY_TYPE d) {
            int rs = 0;
            DELIVERY_TYPE dt = new DELIVERY_TYPE();
            dt.DELIVERY_TYPE_ID = createID();
            dt.DELIVERY_TYPE_NAME = d.DELIVERY_TYPE_NAME;
            dt.DESCRIPTION = d.DESCRIPTION;
            Entities.DELIVERY_TYPE.Add(dt);
            int row = Entities.SaveChanges();
            if (row >= 1)
            {
                rs = row;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult edit(int id)
        {
            DELIVERY_TYPE dt = Entities.DELIVERY_TYPE.Find(id);
            return View(dt);
        }

        [HttpPost]
        public JsonResult edit(DELIVERY_TYPE dt)
        {
            DELIVERY_TYPE d = Entities.DELIVERY_TYPE.Find(dt.DELIVERY_TYPE_ID);
            d.DELIVERY_TYPE_NAME = dt.DELIVERY_TYPE_NAME;
            d.DESCRIPTION = dt.DESCRIPTION;
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
            var x = Entities.DELIVERY_TYPE.Find(id);
            var lst = Entities.DELIVERY_ITEM.Where(model => model.DELIVERY_TYPE_ID == id).ToList();
            if(lst.Count != 0)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Entities.DELIVERY_TYPE.Remove(x);
                int row = Entities.SaveChanges();
                rs = row;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
    }
}