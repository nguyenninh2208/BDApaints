using System;
using System.Linq;
using System.Web.Mvc;
using BDApaint.Model;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity.Validation;

namespace BDApaint.Controllers
{
    public class AdminController : Controller
    {
        private const string V = "dd/MM/yyyy HH:mm:ss tt";
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult checkAdminLogin()
        {
            var message = "";
            if (Session["admin"] != null)
            {
                message = Session["admin"].ToString();
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getLogin(String myString)
        {
            string message = "";

            if (myString == "")
            {
                message = "";
            }
            else
            {
                string[] Arraydata = myString.Split(' ');
                var usernamee = Arraydata[0];
                var passwordd = Arraydata[1];
                ADMIN admin = Entities.ADMINs.Where(x => x.USERNAME == usernamee && x.PASSWORD.Trim() == passwordd).Select(x => x).SingleOrDefault();

                if (admin != null)
                {
                    Session["admin"] = admin.ADMIN_ID;
                    Session["adminName"] = VietHoa(admin.USERNAME);
                    message = "success";
                }
                else
                {

                    message = "fail";
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public static string VietHoa(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            string result = "";

            //lấy danh sách các từ  

            string[] words = s.Split(' ');

            foreach (string word in words)
            {
                // từ nào là các khoảng trắng thừa thì bỏ  
                if (word.Trim() != "")
                {
                    if (word.Length > 1)
                        result += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
                    else
                        result += word.ToUpper() + " ";
                }

            }
            return result.Trim();
        }
        public ActionResult getAdmin()
        {
            var name = Session["adminName"].ToString() ?? "";
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getProduct()
        {
            List<PRODUCER_TYPE> lpt = Entities.PRODUCER_TYPE.ToList();
            ViewBag.lpt = lpt;
            
            return View(Entities.PRODUCTs);
        }
       

        #region sua
        public ActionResult getProductByIDToEdit(int id)
        {
            List<PRODUCER_TYPE> pt = Entities.PRODUCER_TYPE.ToList();
            SelectList sl = new SelectList(pt, "ID", "NAME");
            ViewBag.slp = sl;
            var model = Entities.PRODUCTs.Find(id);
            return View(model);
        }



        //create
        [HttpGet]
        public ActionResult create()
        {
            List<PRODUCER_TYPE> listt = Entities.PRODUCER_TYPE.ToList();
            SelectList seList = new SelectList(listt, "ID", "NAME");
            ViewBag.s = seList;

            return View("createNewProduct");
        }

        [HttpPost]

        public ActionResult EditProduct()
        {
            try
            {
                var product = Entities.PRODUCTs.Find(int.Parse(Request["PRODUCT_ID"]));
                product.PRODUCT_NAME = Request["PRODUCT_NAME"];
                product.DESCRIPTION = Request["DESCRIPTION"];
                product.PRICE = int.Parse(Request["PRICE"]);
                product.TYPE_PRODUCER_ID = int.Parse(Request["TYPE_PRODUCER_ID"]);
                product.AMOUNT = int.Parse(Request["AMOUNT"]);
                //product.DATE_OF_MANUFACTURE = DateTime.Parse(Request["DATE_OF_MANUFACTURE"]);
                product.LIMITED_USE = int.Parse(Request["LIMITED_USE"]);
                product.NET = int.Parse(Request["NET"]);


                HttpPostedFileBase baseFile = Request.Files["IMAGE"];
                if (baseFile != null && baseFile.ContentLength > 0)
                {
                    string filename = Server.MapPath("~/Content/img2/" + baseFile.FileName);
                    baseFile.SaveAs(filename);
                    product.IMAGE = baseFile.FileName;
                }
                Entities.SaveChanges();
                return RedirectToAction("getProduct");

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
            return RedirectToAction("getProduct");
        }
        #endregion

        public ActionResult getUSER()
        {
            return View(Entities.USERs);
        }

        public ActionResult DetailProduct(int? id)
        {
            return View(Entities.PRODUCTs.Find(id));

        }

        // sua


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(PRODUCT entity)
        {
            if (entity == null) return Content("Null cmnr");
            if (ModelState.IsValid)
            {
                try
                {
                    PRODUCT model = Entities.PRODUCTs.Find(entity.PRODUCT_NAME);

                    Entities.SaveChanges();

                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                return RedirectToAction("getTBGameAccount");
            }
            return View(entity);
        }

        public int CreateID() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.PRODUCTs.Find(id) != null)
            {

                return CreateID();

            }
            if (id < 1)
                return CreateID();
            return id;
        }

        public JsonResult checkValidImage(string imgname)
        {
            bool f = true;
            string[] arrayPermit = new string[] { "png", "jpg", "jpeg", "png.png", " " };
            string imgExtentsion = System.IO.Path.GetExtension(imgname);
            if (!Array.Exists(arrayPermit, x => x.Equals(imgExtentsion)))
            {
                f = false;
            }

            var list = Entities.PRODUCTs.ToArray();
            if (Array.Exists(list, x => x.IMAGE == imgname))
            {
                f = false;
            }
            return Json(f, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsAlreadyProduct(string proName)
        {
            Boolean statuss = true;
            if (proName == null)
            {
                statuss = false;
            }
            var lst = Entities.PRODUCTs.ToArray();

            if (Array.Exists(lst, x => x.PRODUCT_NAME.ToUpper() == proName.ToUpper()))
            {
                statuss = false;
            }
            return Json(statuss, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(PRODUCT pro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pro.PRODUCT_ID = CreateID();

                    HttpPostedFileBase baseFile = Request.Files["IMAGE"];
                    if (baseFile != null && baseFile.ContentLength > 0)
                    {
                        string filename = Server.MapPath("~/Content/img2/" + baseFile.FileName);
                        baseFile.SaveAs(filename);
                        pro.IMAGE = baseFile.FileName;
                    }

                    Entities.PRODUCTs.Add(pro);

                    Entities.SaveChanges();
                    return RedirectToAction("getProduct");
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

            return RedirectToAction("create");
        }

        //delete san pham

        public ActionResult deleteProduct(int id)
        {
            var pro = Entities.PRODUCTs.Where(x => x.PRODUCT_ID == id).First();
            Entities.PRODUCTs.Remove(pro);
            Entities.SaveChanges();
            var lst = Entities.PRODUCTs.ToList();
            return View("getProduct", lst);
        }

        //CREATE NEW ORDER IMPORTx`

       

        
        //them supplier mới

        
    }
       


}