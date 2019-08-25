using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BDApaint.Model;
using System.Data.Entity.Validation;
using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace BDApaint.Controllers
{
    public class UserController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        // GET: /User/



        public ActionResult Register()
        {
            return View();
        }
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
        [HttpPost]
        public JsonResult IsAlreadyUsername(string username)
        {
            Boolean statuss = true;
            if (username == null)
            {
                statuss = false;
            }
            var lst = Entities.USERs.ToArray();

            if (Array.Exists(lst, x => x.USERNAME.ToUpper() == username.ToUpper()))
            {
                statuss = false;
            }
            return Json(statuss, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsAlreadyEmail(string email)
        {

            Boolean statuss = true;
            if (email == null)
            {
                statuss = false;
            }
            var lst = Entities.USERs.ToArray();

            if (Array.Exists(lst, x => x.EMAIL.ToUpper() == email.ToUpper()))
            {
                statuss = false;
            }
            return Json(statuss, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(USER user)
        {

            if (ModelState.IsValid)
            {
                USER use = new USER();
                use.ID = CreateID();
                use.FULLNAME = user.FULLNAME;
                use.USERNAME = user.USERNAME;
                use.MEMBERSHIP_TYPE_ID = 0;
                use.PASSWORD = MD5Hash(user.PASSWORD);
                use.PHONE = user.PHONE;
                use.TOTAL_TRANSACTION = 0;
                use.ADDRESS = user.ADDRESS;
                use.EMAIL = user.EMAIL;
                use.DATE_REGISTER = DateTime.Now;
                use.GENDER = user.GENDER;
                Entities.USERs.Add(use);
                Entities.SaveChanges();
                ModelState.Clear();
                Session["UserID"] = user.ID;
                Session["UserName"] = user.USERNAME;
                setAlert("Đăng ký thành công, vui lòng xem thông tin bên dưới", "success");
                return RedirectToAction("UserProfile");

            }
            setAlert("Đăng ký thất bại, vui lòng thử lại", "error");
            return RedirectToAction("Register");

        }
        public ActionResult UserProfile()
        {
            String ssID = Session["UserID"].ToString();
            if (ssID == null)
            {
                return HttpNotFound();
            }
            int IntssID = Int32.Parse(ssID);
            var model = Entities.USERs.Find(IntssID);

            var sumMoneySuccess = model.CARTs.Where(x => x.STATUS == 1).Sum(x => x.TOTAL_MONEY);
            ViewBag.sumMoneySuccess = sumMoneySuccess;
            ViewBag.countSuccess = model.CARTs.Where(x => x.STATUS == 1).Count();

            var sumMoneyWaiting = model.CARTs.Where(z => z.STATUS == 0).Sum(z => z.TOTAL_MONEY);
            ViewBag.sumMoneyWaiting = sumMoneyWaiting;
            ViewBag.countWaiting = model.CARTs.Where(x => x.STATUS == 0).Count();

            var sumMoneyCancel = model.CARTs.Where(cancel => cancel.STATUS == -1).Sum(cancel => cancel.TOTAL_MONEY);
            ViewBag.sumMoneyCancel = sumMoneyCancel;
            ViewBag.countCancel = model.CARTs.Where(x => x.STATUS == -1).Count();
            return View(model);


        }
        public ActionResult Login()
        {
            return PartialView("_LoginPage");
        }
        [HttpPost]
        public ActionResult getLogin(String username,String password )
        {
            var rs = "";
            var x = Entities.USERs.Where(model => model.USERNAME.Equals(username)).FirstOrDefault();
            if (x != null)
            {
                if (x.PASSWORD == MD5Hash(password))
                {
                    rs = "success";
                    Session["UserID"] = x.ID;
                    Session["UserName"] = VietHoa(x.USERNAME);
                    Session["fullName"] = x.FULLNAME;
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    rs = "!Password";
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                rs = "!Username";
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
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

        //Kiểm tra trạng thái đăng nhập
        public ActionResult checkLogined()
        {
            var status = Session["UserID"] ?? 0;
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSignOut()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");
        }
        public int CreateID() // Cơ chế sinh mã
        {
            int id = 0;
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int a = rand.Next() % 10;
                a = a < 0 ? -1 * a : a;
                id += a;
            }
            USER user = Entities.USERs.Find(id);
            if (user != null)
                return CreateID();
            return id;
        }
        //public USER CheckUserName(string username)
        //{
        //    USER user = Entities.USERs.Where(x => x.USERNAME == username).First();
        //    return user;
        //}
        //public USER CheckEmail(string email)
        //{
        //    USER user = Entities.USERs.Where(x => x.EMAIL == email).First();
        //    return user;
        //}

        public ActionResult getCommentUser(int id)
        {
            if (id == 0) return HttpNotFound();
            try
            {
                var cmt = Entities.COMMENTs.Where(p => p.User_ID == id).Select(x => new
                {
                    idcmt = x.Comment_ID,
                    idsp = x.Product_ID,
                    hinhsp = x.PRODUCT.IMAGE,
                    tensp = x.PRODUCT.PRODUCT_NAME,
                    noidung = x.Comment_Content,
                    time = x.date.ToString()
                }).OrderBy(x => x.time).ToList();
                return Json(cmt, JsonRequestBehavior.AllowGet);
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


        //lấy giỏ hàng theo sắp xếp onchange
        public ActionResult getOrderUser(int id, string optionsOrderCart)
        {
            if (id == 0) return HttpNotFound();
            try
            {
                if (optionsOrderCart == "tatca")
                {
                    var orderAll = Entities.CARTs.Where(p => p.USER_ID == id).Select(x => new
                    {
                        idgiohang = x.CART_ID,
                        iduser = x.USER_ID,
                        ngaydat = x.DATE_ORDER.ToString(),
                        lidohuy = x.REASON_CANCEL,
                        tongtien = x.TOTAL_MONEY,
                        trangthai = x.STATUS,
                        giamGiaCart = x.DISCOUNT_VALUE
                    }).OrderBy(x => x.idgiohang).ToList();
                    return Json(orderAll, JsonRequestBehavior.AllowGet);
                }
                else if (optionsOrderCart == "danggiaodich")
                {
                    var orderDangGiaoDich = Entities.CARTs.Where(p => p.USER_ID == id).Select(x => new
                    {
                        idgiohang = x.CART_ID,
                        iduser = x.USER_ID,
                        ngaydat = x.DATE_ORDER.ToString(),
                        lidohuy = x.REASON_CANCEL,
                        tongtien = x.TOTAL_MONEY,
                        trangthai = x.STATUS,
                        giamGiaCart = x.DISCOUNT_VALUE
                    }).Where(x => x.trangthai == 0).OrderBy(x => x.idgiohang).ToList();
                    return Json(orderDangGiaoDich, JsonRequestBehavior.AllowGet);
                }
                else if (optionsOrderCart == "thanhcong")
                {

                    var orderThanhCong = Entities.CARTs.Where(p => p.USER_ID == id).Select(x => new
                    {
                        idgiohang = x.CART_ID,
                        iduser = x.USER_ID,
                        ngaydat = x.DATE_ORDER.ToString(),
                        lidohuy = x.REASON_CANCEL,
                        tongtien = x.TOTAL_MONEY,
                        trangthai = x.STATUS,
                        giamGiaCart = x.DISCOUNT_VALUE
                    }).Where(x => x.trangthai == 1).OrderBy(x => x.idgiohang).ToList();

                    return Json(orderThanhCong, JsonRequestBehavior.AllowGet);
                }
                else if (optionsOrderCart == "dahuy")
                {
                    var orderDaHuy = Entities.CARTs.Where(p => p.USER_ID == id).Select(x => new
                    {
                        idgiohang = x.CART_ID,
                        iduser = x.USER_ID,
                        ngaydat = x.DATE_ORDER.ToString(),
                        lidohuy = x.REASON_CANCEL,
                        tongtien = x.TOTAL_MONEY,
                        trangthai = x.STATUS,
                        giamGiaCart = x.DISCOUNT_VALUE
                    }).Where(x => x.trangthai == -1).OrderBy(x => x.idgiohang).ToList();
                    return Json(orderDaHuy, JsonRequestBehavior.AllowGet);
                }
                var order = Entities.CARTs.Where(p => p.USER_ID == id).Select(x => new
                {
                    idgiohang = x.CART_ID,
                    iduser = x.USER_ID,
                    ngaydat = x.DATE_ORDER.ToString(),
                    lidohuy = x.REASON_CANCEL,
                    tongtien = x.TOTAL_MONEY,
                    trangthai = x.STATUS,
                    giamGiaCart = x.DISCOUNT_VALUE
                }).Where(x => x.trangthai == -1).OrderBy(x => x.idgiohang).ToList();
                return Json(order, JsonRequestBehavior.AllowGet);
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


        //hiển thị giỏ hàng ở mode ready
        public ActionResult getOrderReady(int id)
        {
            if (id == 0) return HttpNotFound();
            try
            {
                var order = Entities.CARTs.Where(p => p.USER_ID == id).Select(x => new
                {
                    idgiohang = x.CART_ID,
                    iduser = x.USER_ID,
                    ngaydat = x.DATE_ORDER.ToString(),
                    lidohuy = x.REASON_CANCEL,
                    tongtien = x.TOTAL_MONEY,
                    trangthai = x.STATUS,
                    giamGiaCart = x.DISCOUNT_VALUE
                }).OrderBy(x => x.idgiohang).ToList();
                return Json(order, JsonRequestBehavior.AllowGet);
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

        //trả về modal hủy
        public ActionResult getOrderCancel(int id)
        {
            if (id == 0) return HttpNotFound();
            try
            {
                var cancel = Entities.CARTs.Where(p => p.CART_ID == id).Select(x => new
                {
                    IDgiohang = x.CART_ID,
                    liDoHuy = x.REASON_CANCEL,
                    trangThai = x.STATUS
                }).OrderBy(x => x.IDgiohang).ToList();
                return Json(cancel, JsonRequestBehavior.AllowGet);
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




        //lấy chi tiết giỏ hàng ( cart item)
        public ActionResult getCartItem(int id)
        {
            if (id == 0) return HttpNotFound();
            try
            {
                var order = Entities.CART_ITEM.Where(p => p.CART_ID == id).Select(x => new
                {
                    CART_ID = x.CART_ID,
                    PRODUCT_ID = x.PRODUCT_ID,
                    PRODUCT_NAME = x.PRODUCT_NAME,
                    IMAGE = x.IMAGE,
                    PRICE = x.PRICE,
                    UNIT_PRICE = x.UNIT_PRICE,
                    QUANTITY = x.QUANTITY,
                    TOTAL_MONEY = x.TOTAL_MONEY,
                    DISCOUNT_VALUE = x.PRODUCT.PRODUCT_DISCOUNT.DISCOUNT_VALUE
                }).OrderBy(x => x.PRODUCT_ID).ToList();
                return Json(order, JsonRequestBehavior.AllowGet);
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


        public USER CheckPassword(string pass)
        {
            USER user = Entities.USERs.Where(x => x.PASSWORD == pass).FirstOrDefault();
            return user;
        }
        public USER CheckEmail(string email)
        {
            USER user = Entities.USERs.Where(x => x.EMAIL == email).FirstOrDefault();
            return user;
        }



        [HttpPost]
        public ActionResult UpdatePassWord()
        {

            try
            {
                var user = Entities.USERs.Find(int.Parse(Request["PRODUCT_ID"]));
                //password ton tai trong db
                string passOld = Request["old_pass"];
                if (CheckPassword(passOld) != null)
                {
                    //Nhập lại mật khẩu trùng với nhập mật khẩu mới
                    if (Request["renewpass"] == Request["newpass"])
                    {
                        user.PASSWORD = Request["newpass"];
                        Entities.SaveChanges();
                        TempData["Message"] = "Đổi mật khẩu thành công!";
                        //return RedirectToAction("UserProfile", "User");
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        //return Content("<script>alert(\"Vui lòng nhập lại mật khẩu  !\")</script>");
                        TempData["Message"] = "Mật khẩu mới không trùng khớp . Vui lòng nhập lại!";
                    }
                }
                else if (CheckPassword(user.PASSWORD) == null)
                {
                    //return Content("<script>alert(\"Mật khẩu cũ sai.Vui lòng nhập lại mật khẩu cũ  !\")</script>");
                    TempData["Message"] = "Mật khẩu cũ sai . Vui lòng nhập lại mật khẩu cũ";
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
            //return RedirectToAction("UserProfile");
            TempData["Message"] = "Đổi mật khẩu thất bại. Đã có lỗi xảy ra!";
            return Redirect(Request.UrlReferrer.ToString());
        }




        public ActionResult EditUser(int id)
        {
            var model = Entities.USERs.Find(id);
            return PartialView("_EditUser", model);
        }
        [HttpPost]
        public ActionResult EditUser()
        {
            try
            {
                var mes = "";
                var user = Entities.USERs.Find(int.Parse(Request["PRODUCT_ID"]));
                user.FULLNAME = Request["fullname"];
                if (Request.Form["Gender"] == "Nam")
                {
                    user.GENDER = 1;
                }
                else
                {
                    user.GENDER = 2;
                }
                user.PHONE = Request.Form["phone"];
                user.ADDRESS = Request["address"];
                user.EMAIL = Request["email"];
                Entities.SaveChanges();
                mes = "Bạn đã sửa thông tin thành công";
                ViewBag.mes = mes;
                return RedirectToAction("UserProfile", "User");

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
            return RedirectToAction("UserProfile");

        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }


        public void SendLinkEmail(string emailID, string code)
        {
            var veriUrl = "/User/ResetPassword/" + code;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, veriUrl);
            var fromEmail = new MailAddress("andromeda123123@gmail.com", "Reset Password");
            var fromEmailPass = "andromeda123";
            var toEmail = new MailAddress(emailID);
            string subject = "";
            string body = "";
            subject = "Reset Password";
            body = "Chào , <br></br>Chúng tôi gửi đến bạn 1 đường dẫn để khôi phục mật khẩu . Vui lòng đi đến đường link dưới đây để đặt mật khẩu mới ." + "<br></br><a href=" + link + ">Request Password link</a>";

            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"].ToString());

            var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPass)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

        }




        [HttpPost]
        public ActionResult ForgotPasswordPost()
        {
            string mailforgot = Request["mailforgot"];
            string message = "";
            var user = Entities.USERs.Where(x => x.EMAIL == mailforgot).FirstOrDefault();
            if (user != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendLinkEmail(user.EMAIL, resetCode);
                user.RESET_PASSWORD_CODE = resetCode;
                //Tránh xác nhận mật khẩu không khớp
                Entities.Configuration.ValidateOnSaveEnabled = false;
                Entities.SaveChanges();
                message = "Link khôi phục mật khẩu đã được gửi đến email của bạn!";
            }
            else
            {
                message = "Không tìm thấy tài khoản nào có email này!";
            }
            ViewBag.message = message;
            return View("ForgotPassword");
        }


        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            var user = Entities.USERs.Where(x => x.RESET_PASSWORD_CODE == id).FirstOrDefault();
            if (user != null)
            {
                ResetPassword model = new ResetPassword();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }



        [HttpPost]
        public ActionResult ResetPassword(ResetPassword model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = Entities.USERs.Where(x => x.RESET_PASSWORD_CODE == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.PASSWORD = MD5Hash(model.NewPassword);
                    user.RESET_PASSWORD_CODE = "";
                    Entities.Configuration.ValidateOnSaveEnabled = false;
                    Entities.SaveChanges();
                    message = "Mật khẩu mới đã được cập nhật thành công!";
                }
            }
            else
            {
                message = "Đã có lỗi xảy ra!";
            }
            ViewBag.message = message;
            return View(model);
        }





        public ActionResult DeleteCart(int id)
        {
            var lstCart = Entities.CARTs.Find(id);
            return PartialView("_DeleteCart", lstCart);
        }

        [HttpPost]
        public ActionResult DeleteCart()
        {
            try
            {
                var mes = "";
                var cart = Entities.CARTs.Find(int.Parse(Request["CART_ID"]));
                cart.STATUS = -1;
                cart.REASON_CANCEL = Request["ReasonCancel"];
                Entities.SaveChanges();
                mes = "Bạn đã hủy thành công đơn hàng này";
                ViewBag.mesDelCart = mes;
                return Redirect(Request.UrlReferrer.ToString());

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
            return RedirectToAction("UserProfile");
        }








    }
}
