using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;
using System.Data.Entity.Validation;

namespace BDApaint.Controllers
{
   
    public class CalculatorController : Controller
    {
        private DATABASELVTN1Entities db = new DATABASELVTN1Entities();
        //
        // GET: /Calculator/
        public ActionResult Index()
        {
            return View();
        }
	}
}