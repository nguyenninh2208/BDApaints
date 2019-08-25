using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;
using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;
using System.Configuration;
namespace BDApaint.Controllers
{
    public class CartItemController : Controller
    {

        private DATABASELVTN1Entities db = new DATABASELVTN1Entities();
        //
        // GET: /CartItem/

        public ViewResult Index()
        {
            TempData["count"] = getAllItem().Count;
            ViewBag.sumTotal = sumTotalMoney();
            ViewBag.sumQuantity = sumQuantity();
            return View("Index", getAllItem());
        }

        public List<CART_ITEM> getAllItem()
        {
            List<CART_ITEM> cart = Session["giohang"] as List<CART_ITEM>;
            cart = db.CART_ITEM.ToList();
            return cart;
        }

        public ActionResult getAll()
        {
            var x = Session["giohang"] as List<CART_ITEM>;

            return Json(x, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddCart(int id)
        {

            if (Session["giohang"] == null)
            {
                //khởi tạo session giỏ hàng
                Session["giohang"] = new List<CART_ITEM>();
            }
            //lưu vào biến 
            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            //neu gio hang trong 
            if (giohang.FirstOrDefault(m => m.PRODUCT_ID == id) == null)
            {
                Model.PRODUCT sp = db.PRODUCTs.Find(id);
                var productDiscount = db.PRODUCT_DISCOUNT.ToList();
                int? moneyDiscount = sp.PRICE;
                var pro = db.PRODUCT_DISCOUNT.Where(x => x.ID == sp.DISCOUNT).SingleOrDefault();
                if (pro != null)
                {
                    //giảm theo mức tiền cố định
                    if (pro.DISCOUNT_UNIT_ID == 1)
                    {
                        moneyDiscount = (int)sp.PRICE - (int)pro.DISCOUNT_VALUE;
                    }
                    //giảm theo phần trăm
                    else
                    {
                        moneyDiscount = ((int)sp.PRICE - ((int)sp.PRICE * (int)pro.DISCOUNT_VALUE) / 100);
                    }

                    CART_ITEM item = new CART_ITEM()
                    {
                        PRODUCT_ID = id,
                        CART_ITEM_ID = CreateIDCartItem(),
                        PRODUCT_NAME = sp.PRODUCT_NAME,
                        QUANTITY = 1,
                        IMAGE = sp.IMAGE,
                        PRICE = sp.PRICE,
                        DISCOUNT_VALUE=sp.PRODUCT_DISCOUNT.DISCOUNT_VALUE,
                        UNIT_PRICE = moneyDiscount,
                        TOTAL_MONEY = moneyDiscount
                    };
                    if (sp.AMOUNT < item.QUANTITY)
                    {
                        return Content("<script>alert(\"Sản phẩm đã hết hàng !\")</script>");
                    }
                    giohang.Add(item);
                    db.SaveChanges();
                    int countDis = giohang.Count();
                    return Json(countDis, JsonRequestBehavior.AllowGet);
                }
                //else
                //{
                //    moneyDiscount = sp.PRICE;
                //}
                CART_ITEM newItem = new CART_ITEM()
                {
                    PRODUCT_ID = id,
                    CART_ITEM_ID = CreateIDCartItem(),
                    PRODUCT_NAME = sp.PRODUCT_NAME,
                    QUANTITY = 1,
                    IMAGE = sp.IMAGE,
                    PRICE = sp.PRICE,
                    UNIT_PRICE = moneyDiscount,
                    TOTAL_MONEY = moneyDiscount
                };
                if (sp.AMOUNT < newItem.QUANTITY)
                {
                    return Content("<script>alert(\"Sản phẩm đã hết hàng !\")</script>");
                }
                giohang.Add(newItem);
                db.SaveChanges();


            }
            else
            {
                Model.PRODUCT sp = db.PRODUCTs.Find(id);
                // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                CART_ITEM item = giohang.FirstOrDefault(c => c.PRODUCT_ID == id);
                //kiểm tra số lượng sản phẩm tồn
                if (sp.AMOUNT < item.QUANTITY)
                {
                    return Content("<script>alert(\"Sản phẩm đả hết hàng ! Quý khách vui lòng chọn sản phẩm khác\")</script>");
                }
                item.QUANTITY++;
                item.TOTAL_MONEY = item.UNIT_PRICE * item.QUANTITY;
            }
            db.SaveChanges();
            int count = giohang.Count();

            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public PRODUCT CheckDiscountValue(int discount)
        {
            PRODUCT pro = db.PRODUCTs.Where(x => x.PRODUCT_DISCOUNT.DISCOUNT_VALUE == discount).FirstOrDefault();
            return pro;
        }

        public int CreateIDCart() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (db.CARTs.Find(id) != null)
            {

                return CreateIDCart();

            }
            if (id < 1)
                return CreateIDCart();
            return id;
        }
        public int CreateIDCartItem() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (db.CART_ITEM.Find(id) != null)
            {

                return CreateIDCartItem();

            }
            if (id < 1)
                return CreateIDCartItem();
            return id;
        }
        private int sumTotalMoney()
        {
            int sumTotal = 0;
            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            if (giohang != null)
            {
                sumTotal = (int)giohang.Sum(x => x.TOTAL_MONEY);
            }
            ViewBag.sumtotal = sumTotal;
            return sumTotal;

        }

        private int sumQuantity()
        {
            int sumQuantity = 0;
            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            if (giohang != null)
            {
                sumQuantity = (int)giohang.Sum(x => x.QUANTITY);
            }
            return sumQuantity;

        }


        //public ActionResult UpdateQuantity(int id)
        //{
        //    string[] quantites = fc.GetValues("quantity");
        //    List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
        //    for (int i = 0; i < giohang.Count; i++)

        //        giohang[i].QUANTITY = Convert.ToInt32(quantites[i]);
        //        Session["giohang"] = giohang;
        //        return View("Index");
        //}

        public ActionResult EditQuantity(int? id, int newQuantity)
        {

            // tìm carditem muon sua
            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            CART_ITEM itemEdit = giohang.FirstOrDefault(m => m.PRODUCT_ID.Equals(id));
            if (itemEdit != null)
            {
                itemEdit.QUANTITY = newQuantity;


            }
            return Json(newQuantity, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Remove(int? id)
        {
            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            CART_ITEM itemDel = giohang.FirstOrDefault(m => m.PRODUCT_ID.Equals(id));
            int count = giohang.Count;
            if (itemDel != null)
            {

                giohang.Remove(itemDel);

            }
            ViewBag.sumTotal = sumTotalMoney();
            return Json(itemDel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveAll()
        {
            Session["giohang"] = null;
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Payment()
        {


            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            if (Session["UserID"] == null)
            {
                return RedirectToAction("LoginView", "Home");
            }
            else
            {
                //lấy both name của delivery để gán qua view
                var deliveryName = db.DELIVERY_ITEM.ToList();
                ViewBag.deliveryName = deliveryName;


                //lấy id của user = session
                int id = (int)Session["UserID"];
                ViewBag.idUser = id;
                String nameUser = db.USERs.Find(id).USERNAME.ToString();
                ViewBag.nameUser = nameUser;
                if (giohang == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                    return View("Payment", giohang);
            }

        }


        [HttpPost]
        public ActionResult Order()
        {

            int id = int.Parse(Request["USER.ID"]);
            String shipname = Request["shipname"];
            String shipemail = Request["shipemail"];
            String shipphone = Request["shipphone"];
            String shipaddress = Request["shipaddress"];
            String shipmess = Request["shipmess"];

            int delivery = int.Parse(Request.Form["DELIVERY"]);

            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            @ViewBag.total = sumTotalMoney();
            int? total = 0;
            int? orderID = 0;
            int? valueDiscount=0;
            var delivery_cost = db.DELIVERY_ITEM.Find(delivery);


            //tiền ship
            int cost_delivery = (int)delivery_cost.DELIVERY_ITEM_COST;
            //số ngày giao hàng
            int count_day_delivery = (int)delivery_cost.ESTIMATED_DATE_DELIVERY;
            DateTime dayDelivery = DateTime.Now.AddDays(count_day_delivery);
            @ViewBag.costDelivery = cost_delivery;
            var us = db.USERs.Find(id);
            var member = db.MEMBERSHIP_TYPE.Where(x => x.ID == us.MEMBERSHIP_TYPE_ID).SingleOrDefault();
            var discountPrice = @ViewBag.total + cost_delivery;
            if (member != null)
            {
                //giảm giá tiền cố định
                if (member.DISCOUNT_UNIT_ID == 1)
                {
                    discountPrice -= (int)member.DISCOUNT_VALUE; 
                }
                //theo %
                else
                {
                    discountPrice = ((int)discountPrice - (((int)discountPrice*(int)member.DISCOUNT_VALUE) / 100));
                }
                CART order = new CART()
                {
                    USER_ID = id,
                    CART_ID = CreateIDCart(),
                    SHIP_NAME = shipname,
                    SHIP_ADDRESS = shipaddress,
                    SHIP_MOBILE = shipphone,
                    SHIP_EMAIL = shipemail,
                    DELIVERY_ITEM_ID = delivery,
                    SHIP_MESSAGES = shipmess,
                    DATE_ORDER = DateTime.Now,
                    DELIVERY_COST = cost_delivery,
                    ESTIMATED_DATE_DELIVERY = DateTime.Now.AddDays(count_day_delivery),
                    //TOTAL_MONEY = @ViewBag.total + cost_delivery,
                    DISCOUNT_VALUE = member.DISCOUNT_VALUE,
                    TOTAL_MONEY = discountPrice,
                    STATUS = 0
                };
                db.CARTs.Add(order);
                total = order.TOTAL_MONEY;
                valueDiscount = order.DISCOUNT_VALUE;
                foreach (CART_ITEM cart in giohang)
                {
                    //total += ((cart.PRICE * cart.QUANTITY)+cost_delivery) ;
                    orderID = order.CART_ID;
                    CART_ITEM orderDetail = new CART_ITEM()
                    {
                        CART_ID = order.CART_ID,
                        CART_ITEM_ID = cart.CART_ITEM_ID,
                        PRODUCT_ID = cart.PRODUCT_ID,
                        QUANTITY = cart.QUANTITY,
                        PRICE = cart.PRICE,
                        UNIT_PRICE = cart.UNIT_PRICE,
                        TOTAL_MONEY = cart.TOTAL_MONEY,
                        PRODUCT_NAME = cart.PRODUCT_NAME,
                        IMAGE = cart.IMAGE,
                        DISCOUNT_VALUE=order.DISCOUNT_VALUE
                    };
                    db.CART_ITEM.Add(orderDetail);
                }
                db.SaveChanges();
                giohang.Clear();


                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));
                content = content.Replace("{{NameShip}}", shipname);
                content = content.Replace("{{Phone}}", shipphone);
                content = content.Replace("{{Email}}", shipemail);
                content = content.Replace("{{Address}}", shipaddress);
                content = content.Replace("{{MessShip}}", shipmess);
                content = content.Replace("{{Total}}", total.ToString());
                content = content.Replace("{{DateOrder}}", DateTime.Now.ToString());
                content = content.Replace("{{OrderID}}", orderID.ToString());
                content = content.Replace("{{DeliveryDate}}", dayDelivery.ToString());
                if (valueDiscount <= 100)
                {
                    content = content.Replace("{{DiscountValue}}", valueDiscount.ToString() + "%");
                }
                else
                {
                    content = content.Replace("{{DiscountValue}}", valueDiscount.ToString() + "đ");
                }
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                //gửi đến mail khách hàng
                new Common.MailHelper().SendMail(shipemail, "Đơn hàng mới từ BDA", content);
                //gửi đến mail quản trị
                new Common.MailHelper().SendMail(toEmail, "Đơn hàng mới từ BDA", content);
                return RedirectToAction("Compelete", new { id = order.CART_ID });

            }



            //member == null . Xử lí khi khách hàng ko có trong thành viên khuyến mãi
            CART orderNor = new CART()
            {
                USER_ID = id,
                CART_ID = CreateIDCart(),
                SHIP_NAME = shipname,
                SHIP_ADDRESS = shipaddress,
                SHIP_MOBILE = shipphone,
                SHIP_EMAIL = shipemail,
                DELIVERY_ITEM_ID = delivery,
                SHIP_MESSAGES = shipmess,
                DATE_ORDER = DateTime.Now,
                DELIVERY_COST = cost_delivery,
                ESTIMATED_DATE_DELIVERY = DateTime.Now.AddDays(count_day_delivery),
                //TOTAL_MONEY = @ViewBag.total + cost_delivery,
                TOTAL_MONEY = discountPrice,
                STATUS = 0
            };
            db.CARTs.Add(orderNor);
            total = orderNor.TOTAL_MONEY;
            foreach (CART_ITEM cart in giohang)
            {
                //total += ((cart.PRICE * cart.QUANTITY)+cost_delivery) ;
                orderID = orderNor.CART_ID;
                CART_ITEM orderDetail = new CART_ITEM()
                {
                    CART_ID = orderNor.CART_ID,
                    CART_ITEM_ID = cart.CART_ITEM_ID,
                    PRODUCT_ID = cart.PRODUCT_ID,
                    QUANTITY = cart.QUANTITY,
                    PRICE = cart.PRICE,
                    UNIT_PRICE = cart.UNIT_PRICE,
                    TOTAL_MONEY = cart.TOTAL_MONEY,
                    PRODUCT_NAME = cart.PRODUCT_NAME,
                    IMAGE = cart.IMAGE
                };
                db.CART_ITEM.Add(orderDetail);
            }
            db.SaveChanges();
            giohang.Clear();


            string contentNew = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));
            contentNew = contentNew.Replace("{{NameShip}}", shipname);
            contentNew = contentNew.Replace("{{Phone}}", shipphone);
            contentNew = contentNew.Replace("{{Email}}", shipemail);
            contentNew = contentNew.Replace("{{Address}}", shipaddress);
            contentNew = contentNew.Replace("{{MessShip}}", shipmess);
            contentNew = contentNew.Replace("{{Total}}", total.ToString());
            contentNew = contentNew.Replace("{{DateOrder}}", DateTime.Now.ToString());
            contentNew = contentNew.Replace("{{OrderID}}", orderID.ToString());
            contentNew = contentNew.Replace("{{DeliveryDate}}", dayDelivery.ToString());
            var toEmailNew = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            //gửi đến mail khách hàng
            new Common.MailHelper().SendMail(shipemail, "Đơn hàng mới từ BDA", contentNew);
            //gửi đến mail quản trị
            new Common.MailHelper().SendMail(toEmailNew, "Đơn hàng mới từ BDA", contentNew);
            return RedirectToAction("Compelete", new { id = orderNor.CART_ID });

        }


        public ActionResult getDeliveryCost(int id)
        {
            if (id == 0) return HttpNotFound();

            try
            {
                //var lst = db.DELIVERY_ITEM.Where(x => x.DELIVERY_ITEM_ID == id).FirstOrDefault();
                ////int cost_delivery = (int)delivery_cost.DELIVERY_ITEM_COST;
                //int cost =(int)lst.DELIVERY_ITEM_COST;
                //return Json(cost, JsonRequestBehavior.AllowGet);
                DateTime now = DateTime.Now;
                var delivery = db.DELIVERY_ITEM.Where(x => x.DELIVERY_ITEM_ID == id).Select(
                    x => new
                    {
                        costDelivery = x.DELIVERY_ITEM_COST,
                        countDateDelivery = (int)x.ESTIMATED_DATE_DELIVERY
                    }).ToList();
                return Json(delivery, JsonRequestBehavior.AllowGet);
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


        public ActionResult Compelete(int? id)
        {
            List<CART_ITEM> giohang = Session["giohang"] as List<CART_ITEM>;
            CART order = db.CARTs.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            if (giohang == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("OrderSuccess", order);
        }

    }
}