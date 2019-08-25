using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;

namespace BDApaint.Controllers
{
    public class NewsController : Controller
    {
        private DATABASELVTN1Entities db = new DATABASELVTN1Entities();

        // GET: /News/
        
        public ActionResult Index()
        {
            var List = new List<NEWS>();
            List = db.NEWS.Take(4).ToList();
            var typeNew = db.NEWS_TYPE.OrderBy(x=>x.TYPE_NEWS_ID).ToList();
            ViewBag.typeNew = typeNew;

            var newestNew = db.NEWS.OrderByDescending(x => x.NEWS_DATE).ToList();
            ViewBag.newest = newestNew;



            return View(List);
        }
  

        // GET: /News/Details/5   
        public ActionResult getDetails(int id)
        {
            var obj = db.NEWS.SingleOrDefault(x => x.NEWS_ID.Equals(id)); 
            ViewBag.cungchude = NewsCungChuDe(obj.NEWS_TYPE.TYPE_NEWS_ID);
            //cùng tác giả
            var cungTacGia = db.NEWS.SingleOrDefault(x => x.NEWS_ID.Equals(id));
            ViewBag.cungTacGia = NewsCungTacGia(cungTacGia.ADMIN.ADMIN_ID);

            var typeNew = db.NEWS_TYPE.OrderBy(x => x.TYPE_NEWS_ID).ToList();
            ViewBag.typeNew = typeNew;
            var newestNew = db.NEWS.OrderByDescending(x => x.NEWS_DATE).ToList();
            ViewBag.newest = newestNew;
            return View(obj);
        }

        public ActionResult getDetailsNewType(int id)
        {
            var Lst = new List<NEWS>();
            Lst = db.NEWS.Where(x => x.NEWS_TYPE_ID == id).ToList();
            var name = Lst.Select(x => x.NEWS_TYPE.NEWS_NAME).ToString();
            ViewBag.name = name;
            var typeNew = db.NEWS_TYPE.OrderBy(x => x.TYPE_NEWS_ID).ToList();
            ViewBag.typeNew = typeNew;
            var newestNew = db.NEWS.OrderByDescending(x => x.NEWS_DATE).ToList();
            ViewBag.newest = newestNew;
            ViewBag.count = Lst.Count;
            return View("getDetailsNewType",Lst);
        }

        public List<NEWS> NewsCungChuDe(int id) 
        {
            List<NEWS> lst = db.NEWS.Where(x => x.NEWS_TYPE.TYPE_NEWS_ID == id).ToList();
            return lst;
        }

        public List<NEWS> NewsCungTacGia(int id)
        {
            List<NEWS> lstTacGia = db.NEWS.Where(x => x.ADMIN.ADMIN_ID == id).ToList();
            return lstTacGia;
        }



        public ActionResult resultSearchNews(string query)
        {
            var lstNews = db.NEWS.Where(x=>x.NEWS_TITLE.Contains(query) || x.NEWS_SUMMARY.Contains(query));
            //ViewBag.searchString = searchString;
            //ViewBag.count = lstProduct.Count();
            ViewBag.query = query;

            var typeNew = db.NEWS_TYPE.OrderBy(x => x.TYPE_NEWS_ID).ToList();
            ViewBag.typeNew = typeNew;
            var newestNew = db.NEWS.OrderByDescending(x => x.NEWS_DATE).ToList();
            ViewBag.newest = newestNew;

            ViewBag.countNews = lstNews.Count();

            return View("resultSearchNews", lstNews.OrderBy(x=>x.NEWS_ID).ToList());
        }





        [HttpPost]
        public ActionResult getSearchNews()
        {
            String query = Request["searchNews"];
            return RedirectToAction("resultSearchNews", new { query = query });
        }
    }
}
