using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Product_ColorController : Controller
    {

        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        
        // GET: Administrator/Product_Color
        public ActionResult Index()
        {
            var model = Entities.PRODUCT_COLOR.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<PRODUCT> lst = Entities.PRODUCTs.ToList();
            SelectList sl = new SelectList(lst, "PRODUCT_ID", "PRODUCT_NAME");
            ViewBag.product = sl;
            List<COLOR> lst1 = Entities.COLORs.ToList();
            SelectList sl1 = new SelectList(lst1, "COLOR_ID", "COLOR_NAME");
            ViewBag.color = sl1;
            
            return View();
        }

        public int CreateID() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.PRODUCT_COLOR.Find(id) != null)
            {

                return CreateID();

            }
            if (id < 1)
                return CreateID();
            return id;
        }
        [HttpPost]

        public ActionResult Create(PRODUCT_COLOR pc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pc.PRODUCT_COLOR_ID = CreateID();
                    Entities.PRODUCT_COLOR.Add(pc);

                    Entities.SaveChanges();
                    return RedirectToAction("detail", "Product_Color",new { id = pc.PRODUCT_COLOR_ID});
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

            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            List<PRODUCT> lst = Entities.PRODUCTs.ToList();
            SelectList sl = new SelectList(lst, "PRODUCT_ID", "PRODUCT_NAME");
            ViewBag.product = sl;
            List<COLOR> lst1 = Entities.COLORs.ToList();
            SelectList sl1 = new SelectList(lst1, "COLOR_ID", "COLOR_NAME");
            ViewBag.color = sl1;
            var model = Entities.PRODUCT_COLOR.Find(id);
            return View(model);
        }

        [HttpPost]

        public ActionResult Edit(int id)
        {
            try
            {
                var product = Entities.PRODUCT_COLOR.Find(id);
               
                product.COLOR_ID = int.Parse(Request["COLOR_ID"]);
                //product.DATE_OF_MANUFACTURE = DateTime.Parse(Request["DATE_OF_MANUFACTURE"]);
                product.PRODUCT_ID = int.Parse(Request["PRODUCT_ID"]);
                Entities.SaveChanges();
                return RedirectToAction("Index", "Product_Color");

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
            return RedirectToAction("Edit");
        }

        public ActionResult detail(int id)
        {
            var model = Entities.PRODUCT_COLOR.Find(id);
            return View(model);
        }

        public ActionResult delete(int id)
        {
            PRODUCT_COLOR model = Entities.PRODUCT_COLOR.Where(x => x.PRODUCT_COLOR_ID == id).SingleOrDefault();
            Entities.PRODUCT_COLOR.Remove(model);
            Entities.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}