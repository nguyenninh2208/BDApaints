using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public ActionResult Index()
        {
            var model = Entities.SUPPLIERs.ToList();
            return View(model);
        }
        public int CreateID() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.SUPPLIERs.Find(id) != null)
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SUPPLIER supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.ID = CreateID();
                Entities.SUPPLIERs.Add(supplier);
                Entities.SaveChanges();
            }
            var model = Entities.SUPPLIERs.ToList();
            return View("Index",model);
        }

        public ActionResult getSupplierByIDToEdit(int id)
        {
            var model = Entities.SUPPLIERs.Find(id);
            return View(model);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSupplier()
        {
            try
            {
                var supplier = Entities.SUPPLIERs.Find(int.Parse(Request["ID"]));
                supplier.NAME = Request["NAME"];
                
                supplier.ADDRESS = (Request["ADDRESS"]);
                supplier.PHONE = Request["PHONE"];
                //supplier.DATE_OF_MANUFACTURE = DateTime.Parse(Request["DATE_OF_MANUFACTURE"]);
                supplier.EMAIL = Request["EMAIL"];
                supplier.URL = Request["URL"];


                
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
            return RedirectToAction("Index");
        }
        public ActionResult deleteSupplier(int id)
        {
            var pro = Entities.SUPPLIERs.Where(x => x.ID == id).First();
            Entities.SUPPLIERs.Remove(pro);
            Entities.SaveChanges();
            var model = Entities.SUPPLIERs.ToList();
            return View("Index",model);
        }
    }
}