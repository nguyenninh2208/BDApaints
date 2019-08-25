using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Color_GroupController : Controller
    {
        // GET: COLOR_GROUP
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public int CreateIDColorGroup() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.COLOR_GROUP.Find(id) != null)
            {

                return CreateIDColorGroup();

            }
            if (id < 1)
                return CreateIDColorGroup();
            return id;
        }

        public ActionResult Index()
        {
            List<PRODUCER> l = Entities.PRODUCERs.ToList();
            List<PRODUCT_TYPE> l1 = Entities.PRODUCT_TYPE.ToList();
            ViewBag.l1 = l1;
            ViewBag.l = l;
            var model = Entities.COLOR_GROUP.ToList();
            return View(model);
        }

        //create View create COLOR GROUP
        [HttpGet]
            public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COLOR_GROUP c)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    c.COLOR_GROUP_ID = CreateIDColorGroup();
                    Entities.COLOR_GROUP.Add(c);
                    Entities.SaveChanges();

                    var model = Entities.COLOR_GROUP.ToList();
                    return View("Index", model);
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

        //Create VIew edit color group 
        [HttpGet]
        public ActionResult edit(int id)
        {
            var model = Entities.COLOR_GROUP.Find(id);
            return View(model);
        }

        //Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            try
            {
                var cl = Entities.COLOR_GROUP.Find(int.Parse(Request["COLOR_GROUP_ID"]));
                cl.COLOR_GROUP_NAME = Request["COLOR_GROUP_NAME"];
                cl.COLOR_GROUP_CODE = (Request["COLOR_GROUP_CODE"]);
                Entities.SaveChanges();
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
                return RedirectToAction("edit");

            }
            
        }

        //Delete

        public ActionResult Delete(int id)
        {
            COLOR_GROUP cg = Entities.COLOR_GROUP.Find(id);
            Entities.COLOR_GROUP.Remove(cg);
            Entities.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult getDetailById(int id)
        {
            var model = Entities.COLOR_GROUP.Find(id);
            return View("detail",model);
        }

        public ActionResult getColorByGroupId(int id)
        {
            var id_Sub = Entities.SUB_COLOR_GROUP.Where(x => x.COLOR_GROUP_ID == id).Select(x=>x.SUB_COLOR_GROUP_ID).ToList();
            List<PRODUCT> model = new List<PRODUCT>();
            List<COLOR> color = new List<COLOR>();
            List <PRODUCT_COLOR> cl = new List<PRODUCT_COLOR>();
            foreach(var i in id_Sub)
            {
                 color = Entities.COLORs.Where(x => x.COLOR_ID == i).ToList();
            }
            foreach(var y in color)
            {
                cl = Entities.PRODUCT_COLOR.Where(x => x.COLOR_ID == y.COLOR_ID).ToList();
            }
            foreach(var z in cl)
            {
                model = Entities.PRODUCTs.Where(m => m.PRODUCT_ID == z.PRODUCT_ID).ToList();
            }
            return RedirectToAction("Index", "Colors", model);
        }
    }
}