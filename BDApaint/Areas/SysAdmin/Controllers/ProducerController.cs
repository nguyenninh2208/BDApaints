using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class ProducerController : Controller
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        // GET: Producer
        public int CreateIDProducer()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.PRODUCT_TYPE.Find(id) != null)
            {

                return CreateIDProducer();

            }
            if (id < 1)
                return CreateIDProducer();
            return id;
        }
        public ActionResult Index()
        {

            var model = Entities.PRODUCERs.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult create()
        {
            return View("create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PRODUCER p)
        {
            if (ModelState.IsValid)
            {
                p.PRODUCER_ID = CreateIDProducer();
                Entities.PRODUCERs.Add(p);
                Entities.SaveChanges();
                var list = Entities.PRODUCERs.ToList();
                return View("Index", list);
            }
            else
            {
                return RedirectToAction("create");
            }
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            var p = Entities.PRODUCERs.Find(id);
            return View("edit",p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            try
            {
                var p = Entities.PRODUCERs.Find(int.Parse(Request["PRODUCER_ID"]));
                p.PRODUCER_NAME = Request["PRODUCER_NAME"];
                p.URL = Request["URL"];
                p.PHONE = Request["PHONE"];
                p.ADDRESS = Request["ADDRESS"];
                p.EMAIL = Request["EMAIL"];

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

            }
            var x = Entities.PRODUCERs.Find(int.Parse(Request["PRODUCER_ID"]));
            return View("edit", x);
        }

        public ActionResult Delete(int id)
        {
            var p = Entities.PRODUCERs.Find(id);
            Entities.PRODUCERs.Remove(p);
            Entities.SaveChanges();
            var model = Entities.PRODUCERs.ToList();
            return View("Index", model);
        }
    }
}