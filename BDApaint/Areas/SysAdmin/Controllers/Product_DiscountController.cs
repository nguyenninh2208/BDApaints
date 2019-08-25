
using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Product_DiscountController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        // GET: SysAdmin/Product_Discount
        public ActionResult Index()
        {
            var model = Entities.PRODUCT_DISCOUNT.ToList();
            return View(model);
        }

        public int CreateID()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.PRODUCT_DISCOUNT.Find(id) != null)
            {

                return CreateID();

            }
            if (id < 1)
                return CreateID();
            return id;
        }

        [HttpGet]
        public ActionResult create()
        {
            var lstUnit = Entities.DISCOUNT_UNIT.ToList();
            ViewBag.lstUnit = new SelectList(lstUnit, "ID", "DESCRIPTION");
            return View();
        }

        [HttpPost]
        public ActionResult create(PRODUCT_DISCOUNT pro)
        {
            var rs = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (pro.CREATE_DATE > pro.UNTIL_VALID || pro.UNTIL_VALID < DateTime.Now)
                    {
                        rs = "DateIsvalid";
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                    PRODUCT_DISCOUNT pros = new PRODUCT_DISCOUNT()
                    {
                        ID = CreateID(),
                        CREATE_DATE = DateTime.Now,
                        DISCOUNT_VALUE = pro.DISCOUNT_VALUE,
                        UNTIL_VALID = pro.UNTIL_VALID,
                        MINIMUM_ORDER_VALUE = pro.MINIMUM_ORDER_VALUE,
                        MAXIMUM_DISCOUNT_VALUE = pro.MAXIMUM_DISCOUNT_VALUE,
                        COUPON_CODE = pro.COUPON_CODE,
                        DISCOUNT_UNIT_ID = pro.DISCOUNT_UNIT_ID,
                        STATUS = pro.STATUS,
                        DESCRIPTION = pro.DESCRIPTION,
                        
                    };
                    Entities.PRODUCT_DISCOUNT.Add(pros);
                    int row = Entities.SaveChanges();
                    if (row >= 1)
                    {
                        rs = "success";
                        setAlert("Thêm thành công", "success");
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    setAlert("Thêm không thành công", "error");
;                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var mystring = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        mystring += validationError.PropertyName + " : " + validationError.ErrorMessage + "\n";
                    }
                }
                return Content(mystring);
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult delete(int id)
        {
            var member = Entities.PRODUCT_DISCOUNT.Where(x => x.ID == id).SingleOrDefault();
            Entities.PRODUCT_DISCOUNT.Remove(member);
            int row = Entities.SaveChanges();
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        public ActionResult changeStatusDiscount(int id)
        {
            int rs = -1;
            var discount = Entities.PRODUCT_DISCOUNT.Find(id);

            discount.STATUS = !discount.STATUS;
            if (discount.STATUS == true)
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

        [HttpGet]
        public ActionResult edit(int id)
        {
            var lstUnit = Entities.DISCOUNT_UNIT.ToList();
            ViewBag.lstUnit = new SelectList(lstUnit, "ID", "DESCRIPTION");
            var model = Entities.PRODUCT_DISCOUNT.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit(PRODUCT_DISCOUNT pro)
        {
            var rs = "";

            if (ModelState.IsValid)
            {
                try
                {
                    PRODUCT_DISCOUNT newPro = Entities.PRODUCT_DISCOUNT.Find(pro.ID);
                    if (newPro.CREATE_DATE > pro.UNTIL_VALID || pro.UNTIL_VALID < DateTime.Now)
                    {
                        rs = "dateIsvalid";
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                    newPro.COUPON_CODE = pro.COUPON_CODE;
                    newPro.UNTIL_VALID = pro.UNTIL_VALID;
                    newPro.MAXIMUM_DISCOUNT_VALUE = pro.MAXIMUM_DISCOUNT_VALUE;
                    newPro.MINIMUM_ORDER_VALUE = pro.MINIMUM_ORDER_VALUE;
                    newPro.DESCRIPTION = pro.DESCRIPTION;
                    if (pro.STATUS == null)
                    {
                        newPro.STATUS = false;
                    }
                    else
                    {
                        newPro.STATUS = pro.STATUS;
                    }
                    newPro.DESCRIPTION = pro.DESCRIPTION;
                    newPro.DISCOUNT_VALUE = pro.DISCOUNT_VALUE;
                    newPro.DISCOUNT_UNIT_ID = pro.DISCOUNT_UNIT_ID;
                    int row = Entities.SaveChanges();
                    if (row >= 1)
                    {
                        setAlert("Cập nhật thành công,Vui lòng kiểm tra danh sách bên dưới", "success");
                        rs = "success";
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rs = "";
                        setAlert("Cập nhật không thành công", "error");
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    throw;
                }
               
            }
            else
            {
                setAlert("Cập nhật không thành công", "error");
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
    }
}