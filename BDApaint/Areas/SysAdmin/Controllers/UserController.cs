using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Administrator/User
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public ActionResult Index()
        {
            
            var model = Entities.USERs.ToList();
            return View(model);
        }
        public ActionResult detail(int id)
        {
            var User = Entities.USERs.Find(id);
            List<CART> cartUser = Entities.CARTs.Where(x => x.USER_ID == id).ToList();
            int t = 0;
            int n = 0;
            foreach(var i in cartUser)
            {
                t += (int)i.TOTAL_MONEY;
                n += 1;
            }
            ViewBag.total = t;
            ViewBag.n = n;
            ViewBag.cartUser = cartUser;
            return View(User);
        }
       
      
    }
}