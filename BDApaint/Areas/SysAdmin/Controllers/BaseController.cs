using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["admin"];
            if(session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new 
                    RouteValueDictionary(new { Controller = "Admin", Action = "logins", Areas = "Sysadmin" }));
            }
            base.OnActionExecuting(filterContext);
        }
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