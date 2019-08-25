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
    public class ToolController : Controller
    {
        private DATABASELVTN1Entities db = new DATABASELVTN1Entities();
        //
        // GET: /Tool/
        public ActionResult Index()
        {
            return View();
        }

       


	}
}