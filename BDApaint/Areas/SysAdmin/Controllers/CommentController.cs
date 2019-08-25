using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class CommentController : BaseController
    {
        // GET: SysAdmin/Comment
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        public ActionResult Index()
        {
            var list = Entities.COMMENTs.ToList();
            return View(list);
        }
        public ActionResult delete(int id)
        {
            COMMENT cmt = Entities.COMMENTs.Find(id);
            Entities.COMMENTs.Remove(cmt);
            int row = Entities.SaveChanges();
            return Json(row, JsonRequestBehavior.AllowGet);
        }
    }
}