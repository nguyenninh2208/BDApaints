using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.DAO;
using BDApaint.Model;

using PagedList;
namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public ActionResult Index()
        {
            List<PRODUCT> model = Entities.PRODUCTs.ToList();
            return View(model);
        }
        public int CreateIDProduct()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.PRODUCTs.Find(id) != null)
            {
                return CreateIDProduct();
            }
            if (id < 1)
                return CreateIDProduct();
            return id;
        }

        //Tạo mới sản phẩm
        public ActionResult create()
        {
            List<PRODUCER> listt = Entities.PRODUCERs.ToList();
            SelectList seList = new SelectList(listt, "PRODUCER_ID", "PRODUCER_NAME");
            List<PRODUCT_TYPE> listpt = Entities.PRODUCT_TYPE.ToList();
            SelectList seList1 = new SelectList(listpt, "TYPE_ID", "TYPE_NAME");
            var lstDiscount = Entities.PRODUCT_DISCOUNT.ToList();
            ViewBag.lstDiscount = new SelectList(lstDiscount, "ID", "COUPON_CODE");
            ViewBag.s = seList;
            ViewBag.s1 = seList1;
            return View("create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PRODUCT pro)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    if (pro.DATE_OF_MANUFACTURE > DateTime.Now)
                    {
                        setAlert("Ngày sản xuất không được lớn hơn ngày hiện tại", "error");
                        RedirectToAction("create");
                    }
                    pro.PRODUCT_ID = CreateIDProduct();
                    pro.AMOUNT = 0;
                    HttpPostedFileBase baseFile = Request.Files["IMAGE"];
                    if (baseFile != null && baseFile.ContentLength > 0)
                    {
                        string filename = Server.MapPath("~/Content/img2/" + baseFile.FileName);
                        var extend = System.IO.Path.GetExtension(baseFile.FileName).Substring(1);
                        if (extend.ToString() == "jpg" || extend.ToString() == "jpeg" || extend.ToString() == "png")
                        {
                            baseFile.SaveAs(filename);
                            pro.IMAGE = baseFile.FileName;
                        }
                        else
                        {
                            setAlert("Thêm thất bại, vui lòng kiểm tra lại hình đã nhập", "error");
                            return RedirectToAction("create");
                        }
                    }
                    Entities.PRODUCTs.Add(pro);
                    int row = Entities.SaveChanges();
                    if (row >= 1)
                    {
                        setAlert("Thêm sản phẩm thành công", "success");
                        return RedirectToAction("detailProduct", "Product", new { @id = pro.PRODUCT_ID });
                    }
                    else
                    {
                        setAlert("Thêm thất bại, vui lòng kiểm tra thông tin nhập", "error");
                        return RedirectToAction("create");
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors);
                setAlert("Vui lòng kiểm tra dữ liệu nhập", "error");
                return RedirectToAction("create");
            }


        }

        //ham phụ trợ thêm xoá sửa

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

        //sua san pham
        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<PRODUCT_TYPE> pt = Entities.PRODUCT_TYPE.ToList();
            SelectList sl = new SelectList(pt, "TYPE_ID", "TYPE_NAME");
            ViewBag.s1 = sl;
            List<PRODUCER> pt1 = Entities.PRODUCERs.ToList();
            SelectList s2 = new SelectList(pt1, "PRODUCER_ID", "PRODUCER_NAME");
            ViewBag.s = s2;
            var lstDiscount = Entities.PRODUCT_DISCOUNT.ToList();
            ViewBag.lstDiscount = new SelectList(lstDiscount, "ID", "COUPON_CODE");
            var model = Entities.PRODUCTs.Find(id);
            return View("edit", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            try
            {

                var product = Entities.PRODUCTs.Find(int.Parse(Request["PRODUCT_ID"]));
                if (product.DATE_OF_MANUFACTURE > DateTime.Now)
                {
                    setAlert("Ngày sản xuất không được lớn hơn ngày hiện tại", "error");
                    RedirectToAction("edit");
                }
                product.PRODUCT_NAME = Request["PRODUCT_NAME"];
                product.DESCRIPTION = Request["DESCRIPTION"];
                product.PRICE = int.Parse(Request["PRICE"]);
                product.PRODUCT_TYPE_ID = int.Parse(Request["PRODUCT_TYPE_ID"]);
                product.PRODUCER_ID = int.Parse(Request["PRODUCER_ID"]);

                //product.DATE_OF_MANUFACTURE = DateTime.Parse(Request["DATE_OF_MANUFACTURE"]);
                product.LIMITED_USE = int.Parse(Request["LIMITED_USE"]);
                product.NET = int.Parse(Request["NET"]);
                HttpPostedFileBase baseFile = Request.Files["IMAGE"];
                if (baseFile != null && baseFile.ContentLength > 0)
                {
                    string filename = Server.MapPath("~/Content/img2/" + baseFile.FileName);
                    var extend = System.IO.Path.GetExtension(baseFile.FileName).Substring(1);
                    if (extend.ToString() == "jpg" || extend.ToString() == "jpeg" || extend.ToString() == "png")
                    {
                        baseFile.SaveAs(filename);
                        product.IMAGE = baseFile.FileName;
                    }
                    else
                    {
                        setAlert("Thêm thất bại, vui lòng kiểm tra lại hình đã nhập", "success");
                        return RedirectToAction("create");
                    }

                }

                int row = Entities.SaveChanges();
                if (row >= 1)
                {
                    setAlert("Cập nhật sản phẩm thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    setAlert("Cập nhật thất bại", "error");
                    return RedirectToAction("Edit");

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
            return RedirectToAction("Edit");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(PRODUCT pro)
        //{
        //    var rs = "";
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            PRODUCT product = Entities.PRODUCTs.Find(pro.PRODUCT_ID);
        //            product.PRODUCT_NAME = pro.PRODUCT_NAME;
        //            product.DESCRIPTION = pro.DESCRIPTION;
        //            product.PRICE = pro.PRICE;
        //            product.PRODUCT_TYPE_ID = pro.PRODUCT_TYPE_ID;
        //            product.PRODUCER_ID = pro.PRODUCER_ID;
        //            product.DATE_OF_MANUFACTURE = pro.DATE_OF_MANUFACTURE;
        //            product.LIMITED_USE = pro.LIMITED_USE;
        //            product.NET = pro.NET;
        //            product.DISCOUNT = pro.DISCOUNT;
        //            string fileName = Path.GetFileNameWithoutExtension(pro.imageUpload.FileName);
        //            if(fileName == null)
        //            {
        //                rs = "imageNotValid";
        //                return Json(rs, JsonRequestBehavior.AllowGet);
        //            }
        //            string extension = Path.GetExtension(pro.imageUpload.FileName);
        //            fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
        //            pro.IMAGE = fileName;
        //            pro.imageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/img2"), fileName));
        //            int row = Entities.SaveChanges();                   
        //            if (row >= 1)
        //            {
        //                rs = "success";
        //                return Json(rs, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        rs = "validate";
        //        return Json(rs, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(rs, JsonRequestBehavior.AllowGet);
        //}
        //delete san pham
        public ActionResult delete(int id)
        {
            var dao = new PRODUCT_DAO();
            var pro = Entities.PRODUCTs.Find(id);
            if (pro.AMOUNT > 0)
            {
                setAlert("Sản phẩm này vẫn còn, không thể xoá, vui lòng kiểm tra lại", "error");
                return RedirectToAction("Index");
            }
            else if (dao.checkProductInOrder(id) == false)
            {
                setAlert("Sản phẩm này đã nằm trong đơn hàng mua bán, vui lòng kiểm tra sản phẩm khi xoá", "error");
                return RedirectToAction("Index");
            }
            else if (dao.checkProductInImport(id) == false)
            {
                setAlert("Sản phẩm này đã nằm trong đơn nhập hàng, vui lòng kiểm tra sản phẩm khi xoá", "error");
                return RedirectToAction("Index");
            }
            else if (dao.checkComment(id) == false)
            {
                setAlert("Sản phẩm này đã có bình luận, vui lòng kiểm tra sản phẩm khi xoá", "error");
                return RedirectToAction("Index");
            }
            else
            {
                Entities.PRODUCTs.Remove(pro);
                int row = Entities.SaveChanges();
                if (row >= 1)
                {
                    setAlert("Xoá thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    setAlert("Xoá thất bại", "error");
                    return RedirectToAction("Index", "Product");
                }
            }
        }

        public ActionResult getProductByProducer(int id)
        {
            ViewBag.List_Type = Entities.PRODUCT_TYPE.ToList();
            ViewBag.List_Producer = Entities.PRODUCERs.ToList();
            List<PRODUCT> lsp = Entities.PRODUCTs.Where(x => x.PRODUCER_ID == id).ToList();
            return View("showProductByProducer", lsp);
        }

        public ActionResult getProductByType(int id)
        {
            ViewBag.List_Type = Entities.PRODUCT_TYPE.ToList();
            ViewBag.List_Producer = Entities.PRODUCERs.ToList();
            List<PRODUCT> lsp = Entities.PRODUCTs.Where(x => x.PRODUCT_TYPE_ID == id).ToList();
            return View("showProductByTypeID", lsp);
        }

        public ActionResult detailProduct(int id)
        {
            var model = Entities.PRODUCTs.Find(id);
            return View(model);
        }

        public ActionResult getImportDetailByIdProduct(int id)
        {
            List<IMPORT_ORDER_DETAIL> iod = Entities.IMPORT_ORDER_DETAIL.Where(x => x.PRODUCT_ID == id).ToList();
            if (iod != null)
            {

                return View("showImportDetailByIdProduct", iod);
            }
            else
            {
                var Pro = Entities.PRODUCTs.Find(id);
                return View("showImportDetailByIdProduct", Pro);
            }
        }

        public ActionResult UpdateStatus()
        {
            List<PRODUCT> lst = Entities.PRODUCTs.ToList();
            DateTime now = DateTime.Now;
            System.TimeSpan d;
            foreach (var i in lst)
            {
                d = now - (DateTime)i.DATE_OF_MANUFACTURE;
                if (d.TotalDays > 60)
                {
                    i.STATUS = 0;
                }
            }
            var model = Entities.PRODUCTs.ToList();
            Entities.SaveChanges();
            return RedirectToAction("Index", "Product", model);
        }

        public ActionResult selectByStatus(int? selectStatus)
        {
            var model = Entities.PRODUCTs.ToList();

            if (selectStatus == -1)
            {
                return Json(model.Select(x => new
                {
                    PRODUCT_ID = x.PRODUCT_ID,
                    PRODUCT_NAME = x.PRODUCT_NAME,
                    PRICE = x.PRICE,
                    NET = x.NET,
                    AMOUNT = x.AMOUNT,
                    LIMITED_USE = x.LIMITED_USE,
                    IMAGE = x.IMAGE,
                    DATE_OF_MANUFACTURE = x.DATE_OF_MANUFACTURE.ToString(),
                    CODE_AUTH = x.CODE_AUTH,
                    STATUS = x.STATUS
                }).ToList(), JsonRequestBehavior.AllowGet);


            }
            else
            {
                return Json(
                    model.Where(x => x.STATUS == selectStatus).Select(x => new
                    {
                        PRODUCT_ID = x.PRODUCT_ID,
                        PRODUCT_NAME = x.PRODUCT_NAME,
                        PRICE = x.PRICE,
                        NET = x.NET,
                        AMOUNT = x.AMOUNT,
                        LIMITED_USE = x.LIMITED_USE,
                        IMAGE = x.IMAGE,
                        DATE_OF_MANUFACTURE = x.DATE_OF_MANUFACTURE.ToString(),
                        CODE_AUTH = x.CODE_AUTH,
                        STATUS = x.STATUS
                    }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getColorByProductId(int id)
        {
            var lst = Entities.PRODUCT_COLOR.Where(x => x.PRODUCT_ID == id).ToList();
            List<COLOR> color = new List<COLOR>();
            foreach (var i in lst)
            {
                var cl = Entities.COLORs.Where(x => x.COLOR_ID == i.COLOR_ID).SingleOrDefault();
                color.Add(cl);
            }
            return RedirectToAction("Index", "Colors", color);
        }

        public ActionResult UpdateAmount()
        {
            List<PRODUCT> lstproduct = new List<PRODUCT>();
            List<CART> lstCart = Entities.CARTs.Where(x => x.STATUS == 1).ToList();
            List<CART_ITEM> lstItem = new List<CART_ITEM>();
            foreach (var i in lstCart)
            {
                var item = Entities.CART_ITEM.Where(x => x.CART_ID == i.CART_ID).ToList();
                lstItem.InsertRange(lstItem.Count, item);
            }
            foreach (var y in lstItem)
            {
                var pro = Entities.PRODUCTs.Where(x => x.PRODUCT_ID == y.PRODUCT_ID).SingleOrDefault();
                pro.SELL += y.QUANTITY;
                pro.AMOUNT -= y.QUANTITY;
                Entities.SaveChanges();
            }
            return RedirectToAction("Index", "Product");

        }

        public ActionResult changeDiscount(int id)
        {
            var value = Entities.PRODUCTs.Find(id);
            var check = 0;
            if (value.DISCOUNT != null)
            {
                check = (int)value.DISCOUNT;
            }
            else
            {
                check = 0;
            }
            var model = Entities.PRODUCT_DISCOUNT.ToList();
            return Json(model.Where(x => x.ID != 0).Select(x => new
            {
                checkbtn = check,
                idPros = id,
                id = x.ID,
                name = x.COUPON_CODE,
                des = x.DESCRIPTION,
                value = x.DISCOUNT_VALUE,
                type = x.DISCOUNT_UNIT.ID,
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult changeDiscountPro(int idPro, int idDiscount)
        {
            var rs = "";
            PRODUCT pro = Entities.PRODUCTs.Find(idPro);
            pro.DISCOUNT = idDiscount;
            int row = Entities.SaveChanges();
            rs = pro.PRODUCT_DISCOUNT.COUPON_CODE;
            if (row >= 1)
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = "0";
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult updateSL()
        {
            var pro = Entities.PRODUCTs;
            var cart = Entities.CARTs.Where(x => x.STATUS == 1).ToList();
            List<IMPORT_ORDER> import = Entities.IMPORT_ORDER.ToList();
            foreach (var i in cart)
            {
                List<CART_ITEM> ci = Entities.CART_ITEM.Where(x => x.CART_ID == i.CART_ID).ToList();
                if (ci != null)
                {
                    foreach (var y in ci)
                    {
                        PRODUCT prs = Entities.PRODUCTs.Where(x => x.PRODUCT_ID == y.PRODUCT_ID).SingleOrDefault();
                        if (prs != null)
                        {
                            prs.SELL += (int)y.QUANTITY;
                        }

                    }
                }

            }
            foreach (var i in import)
            {
                List<IMPORT_ORDER_DETAIL> ip = Entities.IMPORT_ORDER_DETAIL.Where(x => x.IMPORT_ID == i.ID).ToList();
                if (ip != null)
                {
                    foreach (var y in ip)
                    {
                        PRODUCT prs = Entities.PRODUCTs.Where(x => x.PRODUCT_ID == y.PRODUCT_ID).SingleOrDefault();
                        if (prs != null)
                        {
                            prs.BUYING += (int)y.IMPORT_AMOUNT_REAL;
                        }
                    }
                }
            }
            int rs = Entities.SaveChanges();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}