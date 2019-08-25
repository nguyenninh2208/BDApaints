using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;
using System.Data.Entity.Validation;

namespace BDApaint.Controllers
{
    public class ColorController : Controller
    {
        private DATABASELVTN1Entities db = new DATABASELVTN1Entities();
        //
        // GET: /Color/
        public ActionResult Index()
        {
            var List = db.COLOR_GROUP.OrderBy(c => c.COLOR_GROUP_ID);
            return View(List);
        }

        public ActionResult resultSearchNameColor(string query)
        {
            var lstColor = db.COLORs.Where(x => x.COLOR_NAME.Contains(query));
            ViewBag.nameColor = query;
            ViewBag.count = lstColor.Count();
            return View("resultSearchNameColor", lstColor.ToList());
        }



        [HttpPost]
        public ActionResult getSearchNameColor()
        {
            String query = Request["nameColor"];
            return RedirectToAction("resultSearchNameColor", new { query = query });
        }




        [HttpGet]
        public ActionResult detailCOLOR_GROUP(int? id)
        {
            if (id == null) return HttpNotFound();

            try
            {
                var subColorGroup = db.SUB_COLOR_GROUP.Where(x => x.COLOR_GROUP_ID == id).Select(x => new
                {
                    ma = x.SUB_COLOR_GROUP_ID,
                    code = x.SUB_COLOR_GROUP_CODE,
                    name = x.SUB_COLOR_GROUP_NAME,
                    nameGroupColor = x.COLOR_GROUP.COLOR_GROUP_NAME
                }).ToList();
                return Json(subColorGroup, JsonRequestBehavior.AllowGet);
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
        }


        public ActionResult detailSUB_COLOR_GROUP(int? id)
        {
            if (id == null) return HttpNotFound();

            try
            {

                var color2 = db.COLORs.Where(x => x.SUB_COLOR_GROUP_ID == id).Select(x => new
                {
                    id = x.COLOR_ID,
                    mamau = x.COLOR_CODE,
                    codeRgb = x.COLOR_RGB_NAME,
                    tenmau = x.COLOR_NAME
                }).ToList();
                return Json(color2, JsonRequestBehavior.AllowGet);
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
        }

        public ActionResult showColorId(int? id)
        {
            if (id == null) return HttpNotFound();

            try
            {

                var color = db.COLORs.Where(x => x.COLOR_ID == id).Select(x => new
                {
                    id = x.COLOR_ID,
                    mamau = x.COLOR_CODE,
                    codeRgb = x.COLOR_RGB_NAME,
                    tenmau = x.COLOR_NAME
                }).ToList();
                return Json(color, JsonRequestBehavior.AllowGet);
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
        }

        public ActionResult showColor(int? id) //show color theo id của sản phẩm
        {
            if (id == null) return HttpNotFound();

            try
            {

                var colorX = db.PRODUCT_COLOR.Where(x => x.PRODUCT_ID == id).Select(x => new
                {
                    id = x.COLOR.COLOR_ID,
                    mamau = x.COLOR.COLOR_CODE,
                    codeRgb = x.COLOR.COLOR_RGB_NAME,
                    tenmau = x.COLOR.COLOR_NAME,
                    tensp = x.PRODUCT.PRODUCT_NAME

                }).ToList();
                @ViewBag.countColor = colorX.Count();
                return Json(colorX, JsonRequestBehavior.AllowGet);
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
        }

        //hiển thị news theo group color đã chọn tương ứng

        public ActionResult showNewsByColorGroup(int? id)
        {
            if (id == null) return HttpNotFound();
            try
            {

                var newsColorGroup = db.COLOR_GROUP_NEWS.Where(x => x.COLOR_GROUP_ID == id).Select(x => new
                {
                    idTinTuc = x.NEWS.NEWS_ID,
                    tenNhomMau = x.COLOR_GROUP.COLOR_GROUP_NAME,
                    title = x.NEWS.NEWS_TITLE,
                    summary = x.NEWS.NEWS_SUMMARY,
                    content = x.NEWS.NEWS_CONTENT,
                    imgNews = x.NEWS.NEWS_IMG,
                    adminUpload= x.NEWS.ADMIN.USERNAME,
                    thoiGianDang = x.NEWS.NEWS_DATE.ToString(),
                    chude=x.NEWS.NEWS_TYPE.NEWS_NAME,
                    idChuDe = x.NEWS.NEWS_TYPE.TYPE_NEWS_ID
                }).ToList();
                @ViewBag.newsColorGroup = newsColorGroup.Count();
                return Json(newsColorGroup, JsonRequestBehavior.AllowGet);
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
        }











        public ActionResult showProductColor(int? id)
        {

            if (id == null) return HttpNotFound();

            try
            {
                var product = db.PRODUCT_COLOR.Where(p => p.COLOR_ID == id).Select(x => new
                {
                    tenmau = x.COLOR.COLOR_NAME,
                    img = x.PRODUCT.IMAGE,
                    tensp = x.PRODUCT.PRODUCT_NAME,
                    gia = x.PRODUCT.PRICE,
                    idsp = x.PRODUCT_ID
                }).ToList();
                return Json(product, JsonRequestBehavior.AllowGet);
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

        }



    }
}