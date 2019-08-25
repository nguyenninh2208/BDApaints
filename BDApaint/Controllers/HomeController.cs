
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;
using PagedList;
using BDApaint.DAO;

namespace BDApaint.Controllers
{
    public class HomeController : Controller
    {
        DATABASELVTN1Entities db = new DATABASELVTN1Entities();


        public ActionResult getHeader() // kiểm tra trạng thái đăng nhập để lựa chọn header nào
        {
            if (Session["UserID"] != null)
            {
                return PartialView("_Header2");
            }
            else
            {
                return PartialView("_Header1");
            }
        }
     
        public ActionResult Index()
        {
            //lấy sp mới
            var newProduct = db.PRODUCTs.Where(x => x.STATUS == 1).Take(4).ToList();
            ViewBag.newProduct = newProduct;

            //lấy sp khuyến mãi
            var discountProduct = db.PRODUCTs.Where(x => x.DISCOUNT != null).Take(8).ToList();
            ViewBag.discountProduct = discountProduct;


            //lấy hãng
            var producer = db.PRODUCERs.ToList();
            ViewBag.producer = producer;
            //lấy tin tức mới nhất
            var newestNew = db.NEWS.Take(4).OrderBy(x => x.NEWS_DATE).ToList();
            ViewBag.newest = newestNew;

            //lấy feedback của khách hàng
            var feedback = db.FEEDBACKs.OrderBy(x => x.FEEDBACK_ID).ToList();
            ViewBag.countFeedback = feedback.Count;
            ViewBag.feedback = feedback;
            return View("Index");
        }







        //login View
        public ActionResult loginView()
        {

            return View();
        }
    }
}
      

    
