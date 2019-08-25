using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class MemberShip_PromotionController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        // GET: SysAdmin/MemberShip_Promotion
        public ActionResult Index()
        {
            var model = Entities.MEMBERSHIP_TYPE.ToList();
            return View(model);
        }

        public int CreateID()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.MEMBERSHIP_TYPE.Find(id) != null)
            {

                return CreateID();

            }
            if (id < 1)
                return CreateID();
            return id;
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<DISCOUNT_UNIT> lst = Entities.DISCOUNT_UNIT.ToList();
            SelectList sl = new SelectList(lst, "ID", "DESCRIPTION");
            ViewBag.du = sl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult addPro(MEMBERSHIP_TYPE members)
        {
            var rs = "";
            if (ModelState.IsValid)
            {
                if (members.CREATE_DATE > members.VALID_UNTIL || members.VALID_UNTIL < DateTime.Now)
                {
                    rs = "dateIsvalid";
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
                MEMBERSHIP_TYPE member = new MEMBERSHIP_TYPE()
                {
                    ID = CreateID(),
                    CREATE_DATE = DateTime.Today,
                    MEMBERSHIP_TYPE1 = members.MEMBERSHIP_TYPE1,
                    DISCOUNT_VALUE = members.DISCOUNT_VALUE,
                    DISCOUNT_UNIT_ID = members.DISCOUNT_UNIT_ID,
                    VALID_UNTIL = members.VALID_UNTIL,
                    FREE_SHIPPING_ACTIVE = members.FREE_SHIPPING_ACTIVE,
                    MIN_POINT = members.MIN_POINT,
                    MAX_POINT = members.MAX_POINT,
                };
                Entities.MEMBERSHIP_TYPE.Add(member);
                int row = Entities.SaveChanges();
                if (row >= 1)
                {
                    rs = "success";
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult delete(int id)
        {
            var member = Entities.MEMBERSHIP_TYPE.Where(x => x.ID == id).SingleOrDefault();
            Entities.MEMBERSHIP_TYPE.Remove(member);
            int row = Entities.SaveChanges();
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            List<DISCOUNT_UNIT> lst = Entities.DISCOUNT_UNIT.ToList();
            SelectList sl = new SelectList(lst, "ID", "DESCRIPTION");
            ViewBag.du = sl;
            var model = Entities.MEMBERSHIP_TYPE.Where(x => x.ID == id).SingleOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit(MEMBERSHIP_TYPE member)
        {
            var rs = "";
            if (ModelState.IsValid)
            {
                if (member.CREATE_DATE > member.VALID_UNTIL || member.VALID_UNTIL < DateTime.Now)
                {
                    rs = "dateIsvalid";
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    MEMBERSHIP_TYPE newMember = Entities.MEMBERSHIP_TYPE.Where(x => x.ID == member.ID).SingleOrDefault();
                    newMember.ID = member.ID;
                    newMember.MEMBERSHIP_TYPE1 = member.MEMBERSHIP_TYPE1;
                    newMember.VALID_UNTIL = member.VALID_UNTIL;
                    newMember.DISCOUNT_VALUE = member.DISCOUNT_VALUE;
                    newMember.DISCOUNT_UNIT_ID = member.DISCOUNT_UNIT_ID;
                    newMember.FREE_SHIPPING_ACTIVE = member.FREE_SHIPPING_ACTIVE;
                    int row = Entities.SaveChanges();
                    if (row >= 1)
                    {
                        rs = "success";
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                    {
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                rs = "notValid";
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult changeStatusFreeship(int id)
        {
            int rs = -1;
            var free = Entities.MEMBERSHIP_TYPE.Find(id);
            free.FREE_SHIPPING_ACTIVE = !free.FREE_SHIPPING_ACTIVE;
            if (free.FREE_SHIPPING_ACTIVE == true)
            {
                rs = 1;
            }
            else
            {
                rs = 0;
            }
            int row = Entities.SaveChanges();
            if (row >= 1)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = -1;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }


    }
}