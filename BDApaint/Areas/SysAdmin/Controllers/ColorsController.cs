using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class ColorsController : BaseController
    {
        // GET: Colors
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public int CreateIDColor() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.COLORs.Find(id) != null)
            {
                return CreateIDColor();
            }
            if (id < 1)
                return CreateIDColor();
            return id;
        }

        public ActionResult Index()
        {
            var model = Entities.COLORs.ToList();
            return View(model);
        }

        //CREATE VIEW CREATE COLOR

        [HttpGet]
        public ActionResult Create()
        {
            var x = Entities.SUB_COLOR_GROUP.ToList();
            SelectList sl = new SelectList(x, "SUB_COLOR_GROUP_ID", "SUB_COLOR_GROUP_NAME");
            ViewBag.sl = sl;
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COLOR s)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(s.COLOR_NAME == null || s.COLOR_NAME ==null||s.COLOR_CODE == null || s.COLOR_RGB_NAME == null)
                    {
                        setAlert("Thêm không thành công, thông tin nhập bị thiếu", "error");
                        return RedirectToAction("create");
                    }
                    s.COLOR_ID = CreateIDColor();
                    Entities.COLORs.Add(s);
                    int row = Entities.SaveChanges();
                    if(row >= 1)
                    {
                        setAlert("Thêm thành công, xem chi tiết bên dưới", "success");
                        return RedirectToAction("detail", "Colors", new { id = s.COLOR_ID });
                    }
                    else
                    {
                        setAlert("Thêm không thành công, vui lòng kiểm tra thông tin nhập", "error");
                        return RedirectToAction("create");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return RedirectToAction("Create");
        }
        [HttpGet]
        public ActionResult edit(int id)
        {
            var x = Entities.SUB_COLOR_GROUP.ToList();
            SelectList sl = new SelectList(x, "SUB_COLOR_GROUP_ID", "SUB_COLOR_GROUP_NAME");
            ViewBag.sl = sl;
            var model = Entities.COLORs.Find(id);
            return View(model);
        }

        //Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COLOR s)
        {
            
            try
            {
                if (s.COLOR_NAME == null || s.COLOR_NAME == null || s.COLOR_CODE == null || s.COLOR_RGB_NAME == null)
                {
                    setAlert("Cập nhật không thành công, thông tin nhập bị thiếu", "error");
                    return RedirectToAction("create");
                }
                var cl = Entities.COLORs.Find(s.COLOR_ID);
                cl.COLOR_NAME = s.COLOR_NAME;
                cl.COLOR_CODE = s.COLOR_CODE;
                cl.COLOR_RGB_NAME = s.COLOR_RGB_NAME;
                cl.SUB_COLOR_GROUP_ID = s.SUB_COLOR_GROUP_ID;
                int row = Entities.SaveChanges();
                if (row >= 1)
                {
                    setAlert("Cập nhật thành công, xem chi tiết bên dưới", "success");
                    return RedirectToAction("detail", "Colors", new { id = s.COLOR_ID });
                }
                else
                {
                    setAlert("Cập nhật không thành công, vui lòng kiểm tra thông tin nhập", "error");
                    return RedirectToAction("create");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return RedirectToAction("edit");
            }
        }

        //Delete

        public ActionResult Delete(int id)
        {
            int rs = -1;
            COLOR cg = Entities.COLORs.Find(id);
            var lst = Entities.PRODUCT_COLOR.ToList();
            var check = Entities.PRODUCT_COLOR.Where(x => x.COLOR_ID == id).ToList();
            if(check.Count != 0)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Entities.COLORs.Remove(cg);
                int row = Entities.SaveChanges();
                return Json(row, JsonRequestBehavior.AllowGet);
            }            
        }

        public ActionResult detail(int? id)
        {
            var model = Entities.COLORs.Find(id);
            return View("detail", model);
        }

        public ActionResult getColorByIdSub(int? id)
        {
            var model = Entities.COLORs.Where(x => x.SUB_COLOR_GROUP_ID == id).ToList();
            return View("Index",model);
        }
    }
}