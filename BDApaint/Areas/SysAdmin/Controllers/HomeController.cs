using BDApaint.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class HomeController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        // GET: SysAdmin/Home
        public ActionResult Index()
        {
            var lstCart = Entities.CARTs.ToList();
            ViewBag.countCart = lstCart.Count();
            var totalMoney = 0;
            foreach (var i in lstCart)
            {
                totalMoney += (int)i.TOTAL_MONEY;
            }
            ViewBag.totalMoney = totalMoney;
            return View();
        }
        public ActionResult getUser()
        {
            System.TimeSpan d;
            DateTime now = DateTime.Now;
            var rs = 0;
            var lst = Entities.USERs.ToList();
            List<USER> lstUser = new List<USER>();
            foreach (var i in lst)
            {
                d = now - (DateTime)i.DATE_REGISTER;
                if (d.Days <= 1800)
                {
                    lstUser.Add(i);
                }
                rs = lstUser.Count();
            }
            if (rs != 0)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = 0;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getComment()
        {
            var lstComment = Entities.COMMENTs.ToList();
            var rs = lstComment.Count();
            if (rs != 0)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getCartsuccess()
        {
            var countCart = new int[3];
            countCart[0] = Entities.CARTs.Where(x => x.STATUS == 1).Count();
            countCart[1] = Entities.CARTs.Where(x => x.STATUS == -1).Count();
            countCart[2] = Entities.CARTs.Where(x => x.STATUS == 0).Count();
            var data = JsonConvert.SerializeObject(countCart);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTopUser()
        {
            var lst = Entities.USERs.ToList().OrderByDescending(x => x.TOTAL_TRANSACTION);
            return Json(lst.Where(x => x.TOTAL_TRANSACTION >=1).Select(x => new
            {
                id = x.ID,
                USERNAME = x.USERNAME,
                TOTAL = x.TOTAL_TRANSACTION,
            }).ToList().Take(4), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTopPro()
        {
            var lst = Entities.PRODUCTs.ToList().OrderByDescending(x => x.SELL);
            return Json(lst.Where(x => x.SELL >= 100).Select(x => new
            {
                id = x.PRODUCT_ID,
                name = x.PRODUCT_NAME,
                img  = x.IMAGE,
                sell = x.SELL,
                amount = x.AMOUNT
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUnderPro()
        {
            var lst = Entities.PRODUCTs.ToList().OrderByDescending(x => x.AMOUNT);
            return Json(lst.Where(x => x.SELL == 0 && x.AMOUNT>=300).Select(x => new
            {
                id = x.PRODUCT_ID,
                name = x.PRODUCT_NAME,
                img = x.IMAGE,
                sell = x.SELL,
                amout = x.AMOUNT
            }).ToList().Take(5), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getNoPro()
        {
            var lst = Entities.PRODUCTs.ToList().OrderBy(x => x.AMOUNT);
            return Json(lst.Where(x => x.AMOUNT >= 0 && x.AMOUNT <=100).Select(x => new
            {
                id = x.PRODUCT_ID,
                name = x.PRODUCT_NAME,
                img = x.IMAGE,
                sell = x.SELL,
                amout = x.AMOUNT
            }).ToList().Take(5), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTopTran()
        {
            var lst = Entities.CARTs.ToList().OrderByDescending(x => x.TOTAL_MONEY);
            return Json(lst.Select(x => new
            {
                id = x.CART_ID,
                user = x.USER.USERNAME,
                date = x.DATE_ORDER.ToString(),
                total = x.TOTAL_MONEY,
                
            }).ToList().Take(5), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTraning()
        {
            var lst = Entities.CARTs.ToList().OrderByDescending(x => x.TOTAL_MONEY);
            return Json(lst.Where(x=>x.STATUS == 0).Select(x => new
            {
                id = x.CART_ID,
                user = x.USER.USERNAME,
                date = x.DATE_ORDER.ToString(),
                total = x.TOTAL_MONEY,

            }).ToList().Take(10), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getImportNewest()
        {
            var lst = Entities.IMPORT_ORDER.ToList().OrderByDescending(x => x.DATE_IMPORT);
            return Json(lst.Where(x => x.TOTAL > 0).Select(x => new
            {
                id = x.ID,
                user = x.PERSON_IMPORT.NAME,
                sup = x.SUPPLIER.NAME,
                date = x.DATE_IMPORT.ToString(),
                total = x.TOTAL,
            }).ToList().Take(5), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getImportHighest()
        {
            var lst = Entities.IMPORT_ORDER.ToList().OrderByDescending(x => x.TOTAL);
            return Json(lst.Where(x => x.TOTAL > 0).Select(x => new
            {
                id = x.ID,
                user = x.PERSON_IMPORT.NAME,
                sup = x.SUPPLIER.NAME,
                date = x.DATE_IMPORT.ToString(),
                total = x.TOTAL,
            }).ToList().Take(5), JsonRequestBehavior.AllowGet);
        }
    }
}