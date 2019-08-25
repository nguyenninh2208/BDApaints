using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class NewsController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        // GET: SysAdmin/News
        public ActionResult Index()
        {
            var model = Entities.NEWS.ToList();
            return View(model);
        }
        public int CreateID()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.NEWS.Find(id) != null)
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
            List<NEWS_TYPE> nt = Entities.NEWS_TYPE.ToList();
            SelectList sl = new SelectList(nt, "TYPE_NEWS_ID", "NEWS_NAME");
            ViewBag.tn = sl;
            List<ADMIN> admin = Entities.ADMINs.ToList();
            SelectList sl1 = new SelectList(admin, "ADMIN_ID", "FULL_NAME");
            ViewBag.admin = sl1;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(NEWS news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    news.NEWS_ID = CreateID();
                    news.NEWS_DATE = DateTime.Now;
                    var x = news.STATUS;
                    if (x == null)
                    {
                        news.STATUS = false;
                    }
                    HttpPostedFileBase baseFile = Request.Files["NEWS_IMG"];
                    if (baseFile != null && baseFile.ContentLength > 0)
                    {
                        string filename = Server.MapPath("~/AdminContent/imgNews/" + baseFile.FileName);
                        baseFile.SaveAs(filename);
                        news.NEWS_IMG = baseFile.FileName;
                        var extend = System.IO.Path.GetExtension(baseFile.FileName).Substring(1);
                        if (extend.ToString() == "jpg" || extend.ToString() == "jpeg" || extend.ToString() == "png")
                        {
                            baseFile.SaveAs(filename);
                            news.NEWS_IMG = baseFile.FileName;
                        }
                        else
                        {
                            setAlert("Thêm thất bại, vui lòng kiểm tra lại hình đã nhập", "error");
                            return RedirectToAction("create");
                        }
                    }
                    Entities.NEWS.Add(news);
                    int row = Entities.SaveChanges();
                    if (row >= 1)
                    {
                        setAlert("Thêm mới thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        setAlert("Thêm mới không thành công", "danger");
                        return RedirectToAction("create");
                    }
                    //return RedirectToAction("detailProduct", "Product", new { id = pro.PRODUCT_ID });
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
            setAlert("Thêm mới không thành công", "danger");
            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<NEWS_TYPE> nt = Entities.NEWS_TYPE.ToList();
            SelectList sl = new SelectList(nt, "TYPE_NEWS_ID", "NEWS_NAME");
            ViewBag.tn = sl;
            List<ADMIN> admin = Entities.ADMINs.ToList();
            SelectList sl1 = new SelectList(admin, "ADMIN_ID", "FULL_NAME");
            ViewBag.admin = sl1;
            var x = Entities.NEWS.Find(id);
            ViewBag.ID = id;
            return View(x);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {

            if (ModelState.IsValid)
            {
                int ids = int.Parse(Request["NEWS_ID"]);
                NEWS newss = Entities.NEWS.Find(ids);
                newss.NEWS_TYPE_ID = int.Parse(Request["NEWS_TYPE_ID"]);
                newss.NEWS_SUMMARY = Request["NEWS_SUMMARY"];
                newss.ADMIN_ID = int.Parse(Request["ADMIN_ID"]);
                newss.NEWS_CONTENT = Request["NEWS_CONTENT"];
                HttpPostedFileBase baseFile = Request.Files["NEWS_IMG"];
               
                if (baseFile != null && baseFile.ContentLength > 0)
                {
                    string filename = Server.MapPath("~/AdminContent/imgNews/" + baseFile.FileName);
                    baseFile.SaveAs(filename);
                    newss.NEWS_IMG = baseFile.FileName;
                    var extend = System.IO.Path.GetExtension(baseFile.FileName).Substring(1);
                    if (extend.ToString() == "jpg" || extend.ToString() == "jpeg" || extend.ToString() == "png")
                    {
                        baseFile.SaveAs(filename);
                        newss.NEWS_IMG = baseFile.FileName;
                    }
                    else
                    {
                        setAlert("Thêm thất bại, vui lòng kiểm tra lại hình đã nhập", "error");
                        return RedirectToAction("create");
                    }
                }
                int row = Entities.SaveChanges();
                if (row >= 1)
                {
                    setAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    setAlert("Cập nhật không thành công", "error");
                    return RedirectToAction("edit", new { id = ids });
                }
            }
            else
            {
                setAlert("Cập nhật không thành công, bạn chưa nhập đầy đủ thông tin", "error");
                return RedirectToAction("edit");
            }
        }



        public ActionResult delete(int id)
        {
            var news = Entities.NEWS.Find(id);
            Entities.NEWS.Remove(news);
            int rs = Entities.SaveChanges();
            if (rs >= 1)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = 0;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult changeStatus(int id)
        {
            var rs = -1;
            var news = Entities.NEWS.Find(id);
            if (news.STATUS == null || news.STATUS == false)
            {
                news.STATUS = true;
                rs = 1;
            }
            else
            {
                news.STATUS = false;
                rs = 0;
            }
            int row = Entities.SaveChanges();
            if (row == 0)
            {
                rs = -1;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult detail(int id)
        {
            var news = Entities.NEWS.Find(id);
            return View(news);
        }
    }
}