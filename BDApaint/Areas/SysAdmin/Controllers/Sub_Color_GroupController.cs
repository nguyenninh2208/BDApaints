using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class SUB_COLOR_GROUPController : BaseController
    {
        // GET: SUB_COLOR_GROUP
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public int CreateIDSubColorGroup() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.SUB_COLOR_GROUP.Find(id) != null)
            {

                return CreateIDSubColorGroup();

            }
            if (id < 1)
                return CreateIDSubColorGroup();
            return id;
        }
        public ActionResult Index()
        {

            var model = Entities.SUB_COLOR_GROUP.ToList();
            return View(model);
        }

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
        public ActionResult Create(SUB_COLOR_GROUP s)
        {
            var x = Entities.SUB_COLOR_GROUP.ToList();
            SelectList sl = new SelectList(x, "SUB_COLOR_GROUP_ID", "SUB_COLOR_GROUP_NAME");
            ViewBag.sl = sl;
            try
            {
                if (ModelState.IsValid)
                {
                    s.SUB_COLOR_GROUP_ID = CreateIDSubColorGroup();
                    Entities.SUB_COLOR_GROUP.Add(s);
                    Entities.SaveChanges();

                    var model = Entities.SUB_COLOR_GROUP.ToList();
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

        [HttpGet]
        public ActionResult edit(int id)
        {
            var x = Entities.SUB_COLOR_GROUP.ToList();
            SelectList sl = new SelectList(x, "SUB_COLOR_GROUP_ID", "SUB_COLOR_GROUP_NAME");
            ViewBag.sl = sl;
            var model = Entities.SUB_COLOR_GROUP.Find(id);
            return View(model);
        }

        //Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            var x = Entities.SUB_COLOR_GROUP.ToList();
            SelectList sl = new SelectList(x, "SUB_COLOR_GROUP_ID", "SUB_COLOR_GROUP_NAME");
            ViewBag.sl = sl;
            try
            {
                var cl = Entities.SUB_COLOR_GROUP.Find(int.Parse(Request["SUB_COLOR_GROUP_ID"]));
                cl.SUB_COLOR_GROUP_NAME = Request["SUB_COLOR_GROUP_NAME"];
                cl.SUB_COLOR_GROUP_CODE = (Request["SUB_COLOR_GROUP_CODE"]);
                cl.COLOR_GROUP_ID = int.Parse(Request["COLOR_GROUP_ID"]);
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
            SUB_COLOR_GROUP cg = Entities.SUB_COLOR_GROUP.Find(id);
            Entities.SUB_COLOR_GROUP.Remove(cg);
            Entities.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult getDetailSubById(int id)
        {
            var model = Entities.SUB_COLOR_GROUP.Find(id);
            return View("Detail", model);
        }

        public ActionResult getSubColorByIdColorGroup(int id)
        {
            var model = Entities.SUB_COLOR_GROUP.Where(x => x.COLOR_GROUP_ID == id).ToList();
            return View("Index", model);
        }
    }
}