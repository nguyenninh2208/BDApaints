using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;
using PagedList;
using System.Data.Entity.Validation;
using PusherServer;
namespace BDApaint.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();


        //chi tiet sp 
        public ActionResult getDetailProduct(int? id)
        {
            //lấy sp mới
            var newProduct = Entities.PRODUCTs.Where(x => x.STATUS == 1).ToList().Take(4);
            ViewBag.newProduct = newProduct;

            //lấy sp cùng hãng
            var obj = Entities.PRODUCTs.SingleOrDefault(x => x.PRODUCT_ID == id);
            ViewBag.productProducer = productProducer(obj.PRODUCER.PRODUCER_ID).Take(4);

            //lấy sp khuyến mãi
            var discountProduct = Entities.PRODUCTs.Where(x => x.DISCOUNT != null).Take(8).ToList();
            ViewBag.discountProduct = discountProduct;
            
           
          

            //check null khuyến mãi tránh lỗi object null
            var model = Entities.PRODUCTs.Find(id);
            var pro = Entities.PRODUCT_DISCOUNT.Where(x => x.ID == model.DISCOUNT).SingleOrDefault();

            
            
            if (pro != null)
            {
                //san pham da xem
                var list = Session["SanPhamDaXem"] as List<RECENT_PRODUCT> as List<RECENT_PRODUCT>;
                if (list == null)
                {
                    list = new List<RECENT_PRODUCT>();
                    Session["SanPhamDaXem"] = list;
                }
                // Add product to recent list (make list contain max 5 items)
                AddRecentProduct(list, id.Value, model.PRODUCT_NAME, model.IMAGE, model.PRICE, model.PRODUCT_DISCOUNT.DISCOUNT_VALUE,model.STATUS, 12);
                //gan view data 
                ViewData["SanPhamDaXem"] = list;
                return View(model);
            }
            else
            {
                //san pham da xem
                var list = Session["SanPhamDaXem"] as List<RECENT_PRODUCT> as List<RECENT_PRODUCT>;
                if (list == null)
                {
                    list = new List<RECENT_PRODUCT>();
                    Session["SanPhamDaXem"] = list;
                }
                // Add product to recent list (make list contain max 5 items)
                AddRecentProduct(list, id.Value, model.PRODUCT_NAME, model.IMAGE, model.PRICE, 0,model.STATUS, 12);
                //gan view data 
                ViewData["SanPhamDaXem"] = list;
                model.DISCOUNT = null;
                return View(model);
            }        
        }

        //product recent view

        public void AddRecentProduct(List<RECENT_PRODUCT> list, int id, string name,string img,int? price,int? discountValue,int? status ,int maxItems)
        {
            var item = list.FirstOrDefault(t => t.PRODUCT_ID == id);
            if (item == null)
            {
                var model = Entities.PRODUCTs.Find(id);
                var pro = Entities.PRODUCT_DISCOUNT.Where(x => x.ID == model.DISCOUNT).SingleOrDefault();
                //sp có khuyến mãi
                if (pro != null)
                {
                    // Add item only if it does not exist
                    list.Add(new RECENT_PRODUCT
                    {
                        PRODUCT_ID = id,
                        PRODUCT_NAME = name,
                        IMAGE = img,
                        PRICE = price,
                        DISCOUNT_VALUE = discountValue,
                        STATUS = status,
                        LAST_VISITED = DateTime.Now,
                    });
                }
                else
                {
                    list.Add(new RECENT_PRODUCT
                    {
                        PRODUCT_ID = id,
                        PRODUCT_NAME = name,
                        IMAGE = img,
                        PRICE = price,
                        DISCOUNT_VALUE = 0,
                         STATUS = status,
                        LAST_VISITED = DateTime.Now,
                    });

                }
              
            }
            // Check that recent product list does not exceed maxItems
            while (list.Count > maxItems)
            {
                list.RemoveAt(0);
            }
        }










        public List<PRODUCT> productProducer(int id)
        {
            var thisNew = Entities.PRODUCERs.Find(id);
            string name = thisNew.PRODUCER_NAME;
            var cungchude = Entities.PRODUCTs.Where(x => x.PRODUCER_ID == Entities.PRODUCERs.FirstOrDefault(t => t.PRODUCER_NAME.Equals(name)).PRODUCER_ID).ToList();
            return cungchude;
        }


        public ActionResult resultSearchProduct(string myString)
        {
            var lst = Entities.PRODUCTs.Where(x => x.PRODUCT_NAME.Contains(myString) || x.PRODUCER.PRODUCER_NAME.Contains(myString));
            ViewBag.countLst = lst.Count();
            ViewBag.myString = myString;
            return View("ResultSearchs", lst.OrderBy(x => x.PRODUCT_ID).ToList());
        }




        [HttpPost]
        public ActionResult SearchProduct()
        {

            String myString = Request["SearchKey"];
            return RedirectToAction("resultSearchProduct", new { myString = myString });

        }

        public ActionResult detailProduct(int id)
        {
            var importDetail = Entities.IMPORT_ORDER_DETAIL.Where(x => x.PRODUCT_ID == id).First();
            var import = Entities.IMPORT_ORDER.Where(x => x.ID == importDetail.IMPORT_ID).First();
            var dateimport = import.DATE_IMPORT;
            ViewBag.date = dateimport;
            var total = import.TOTAL;
            ViewBag.total = total;
            var sup = import.SUPPLIER.NAME;
            ViewBag.sup = sup;
            var importID = import.ID;
            ViewBag.importID = importID;
            var sl = importDetail.IMPORT_AMOUNT;
            ViewBag.sl = sl;
            var model = Entities.PRODUCTs.Find(id);
            return View(model);
        }

        public ActionResult DisplayComment(int? productID, string optionsComment)
        {
            if (productID == null)
                return HttpNotFound();
            try
            {
                if (optionsComment == "tatca")
                {
                    var lstAll = Entities.COMMENTs.Where(x => x.Product_ID == productID).Select(x => new
                        {
                            Comment_ID = x.Comment_ID,
                            Product_ID = x.Product_ID,
                            User_ID = x.User_ID,
                            Comment_Content = x.Comment_Content,
                            date = x.date.ToString(),
                            User_Name = x.USER.FULLNAME
                        }).ToList();
                    return Json(lstAll, JsonRequestBehavior.AllowGet);
                }
                else if (optionsComment == "moinhat")
                {
                    var lstMoiNhat = Entities.COMMENTs.Where(x => x.Product_ID == productID).Select(x => new
                    {
                        Comment_ID = x.Comment_ID,
                        Product_ID = x.Product_ID,
                        User_ID = x.User_ID,
                        Comment_Content = x.Comment_Content,
                        date = x.date.ToString(),
                        User_Name = x.USER.FULLNAME
                    }).OrderByDescending(x => x.date).ToList();
                    return Json(lstMoiNhat, JsonRequestBehavior.AllowGet);
                }
                else if (optionsComment == "cunhat")
                {
                    var lstCuNhat = Entities.COMMENTs.Where(x => x.Product_ID == productID).Select(x => new
                    {
                        Comment_ID = x.Comment_ID,
                        Product_ID = x.Product_ID,
                        User_ID = x.User_ID,
                        Comment_Content = x.Comment_Content,
                        date = x.date.ToString(),
                        User_Name = x.USER.FULLNAME
                    }).OrderBy(x => x.date).ToList();
                    return Json(lstCuNhat, JsonRequestBehavior.AllowGet);
                }
                var lstCMT = Entities.COMMENTs.Where(x => x.Product_ID == productID).Select(x => new
                {
                    Comment_ID = x.Comment_ID,
                    Product_ID = x.Product_ID,
                    User_ID = x.User_ID,
                    Comment_Content = x.Comment_Content,
                    date = x.date.ToString(),
                    User_Name = x.USER.FULLNAME
                }).ToList();
                return Json(lstCMT, JsonRequestBehavior.AllowGet);
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
        }

        //hiển thị comment khi ready page
        public ActionResult CommentReady(int? productID)
        {
            if (productID == null)
                return HttpNotFound();
            try
            {
                var lstCMT = Entities.COMMENTs.Where(x => x.Product_ID == productID).Select(x => new
                {
                    Comment_ID = x.Comment_ID,
                    Product_ID = x.Product_ID,
                    User_ID = x.User_ID,
                    Comment_Content = x.Comment_Content,
                    date = x.date.ToString(),
                    User_Name = x.USER.FULLNAME
                }).OrderBy(x => x.date).ToList();
                return Json(lstCMT, JsonRequestBehavior.AllowGet);
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
        }


        [HttpPost]
        public ActionResult AddComment(int? productID)
        {
            COMMENT cmt = null;
            var message = "";
         
            var product = Entities.PRODUCTs.FirstOrDefault(p => p.PRODUCT_ID == productID);
            if (Session["UserID"] == null)
            {
                message = "Bạn chưa đăng nhập . Vui lòng đăng nhập để bình luận";
                return RedirectToAction("LoginView", "Home");
                
            }
            else
            {
                //int id = int.Parse(Request["UserID"]);
                int id = (int)Session["UserID"];
                String contentComment = Request["contentComment"];
                if (contentComment == null)
                {
                    message = "Nội dung bình luận không được để trống ! Vui lòng nhập lại nội dung bình luận!";
                }
                var User = Entities.USERs.FirstOrDefault(u => u.ID == id);
                cmt = new COMMENT
                {
                    Comment_ID = CreateIDComment(),
                    Product_ID = product.PRODUCT_ID,
                    User_ID = id,
                    Comment_Content = contentComment,
                    date = DateTime.Now
                };
                //ko hien thi cmt null
                if (cmt.Comment_Content != null)
                {
                    Entities.COMMENTs.Add(cmt);
                    Entities.SaveChanges();
                    message = "Bình luận thành công";
                }
                ViewBag.message = message;
                //return RedirectToAction("DisplayComment", new { productID = productID });
                //trả về trang đang đứng
                return Redirect(Request.UrlReferrer.ToString());
                //return RedirectToAction("getDetailProduct", new { id = id });
            //return   Json(mes, JsonRequestBehavior.AllowGet);
               
            }
         
        }




        public int CreateIDComment() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.COMMENTs.Find(id) != null)
            {

                return CreateIDComment();

            }
            if (id < 1)
                return CreateIDComment();
            return id;
        }




    }
}