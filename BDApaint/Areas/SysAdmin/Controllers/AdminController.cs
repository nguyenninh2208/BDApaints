using System;
using System.Linq;
using System.Web.Mvc;
using BDApaint.Model;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity.Validation;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class AdminController : Controller
    {
        private const string V = "dd/MM/yyyy HH:mm:ss tt";
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Logins()
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
        public ActionResult getLogin(String username,String password)
        {
            var rs = "";
            var x = Entities.ADMINs.Where(model => model.USERNAME.Equals(username)).FirstOrDefault();
            if (x != null)
            {
                if (x.PASSWORD == EncriptMD5Controller.MD5Hash(password))
                {
                    rs = "success";
                    Session["admin"] = x.ADMIN_ID;
                    Session["adminName"] = VietHoa(x.USERNAME);
                    Session["fullName"] = x.FULL_NAME;
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
        public ActionResult getAdmin()
        {
            var name = Session["fullName"].ToString() ?? "";
            return Json(name, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLogout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["Admin"] = null;
            return RedirectToAction("Logins", "Admin");
        }

        public ActionResult Profiles()
        {
            var x = 0;
            if (Session["admin"] != null)
            {
                x = (int)Session["admin"];
                var model = Entities.ADMINs.Find(x);
                return RedirectToAction("Index", "Admin", model);
            }
            return RedirectToAction("Index", "Home");
        }
    }



}