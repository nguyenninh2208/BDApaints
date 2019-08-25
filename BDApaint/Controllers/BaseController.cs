using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
       
        protected void setAlert(string message, string type)
        {
            TempData["alertMessage"] = message;
            if (type == "success")
            {
                TempData["alertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["alertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["alertType"] = "alert-danger";
            }
        }
    }
}