using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Product_TypeController : Controller
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        // GET: Product_Type
        public ActionResult Index()
        {
            var model = Entities.PRODUCT_TYPE.ToList();
            return View(model);
        }

      
        public int CreateIDProductType()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.PRODUCT_TYPE.Find(id) != null)
            {
                return CreateIDProductType();
            }
            if (id < 1)
                return CreateIDProductType();
            return id;
        }

        [HttpGet]
        public ActionResult create()
        {
            return View("createView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create(PRODUCT_TYPE type)
        {
            
            if (ModelState.IsValid)
            {
                type.TYPE_ID = CreateIDProductType();
                Entities.PRODUCT_TYPE.Add(type);
                Entities.SaveChanges();
            }
            else
                return View("createView");
            List<PRODUCT_TYPE> lst = Entities.PRODUCT_TYPE.ToList();
            return View("Index",lst);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var model = Entities.PRODUCT_TYPE.Find(id);
            return View("edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            int rowEdit = 0;
            try
            {
                var p = Entities.PRODUCT_TYPE.Find(int.Parse(Request["TYPE_ID"]));
                p.TYPE_NAME = Request["TYPE_NAME"];
                p.USES = Request["USES"];

                rowEdit = Entities.SaveChanges();
                return RedirectToAction("Index");
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
                var x = Entities.PRODUCT_TYPE.Find(Request["TYPE_ID"]);
                return View("edit",x);           
        }

        public ActionResult Delete(int? id)
        {
            var pt = Entities.PRODUCT_TYPE.Find(id);
            Entities.PRODUCT_TYPE.Remove(pt);
            Entities.SaveChanges();
            var list = Entities.PRODUCT_TYPE.ToList();
            return View("Index",list);
        }
    }
}