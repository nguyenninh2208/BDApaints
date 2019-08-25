using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity.Validation;

namespace BDApaint.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Send()
        {
            String usernameContact = Request["usernameContact"];
            String emailContact = Request["emailContact"];
            String subjectContact = Request["subjectContact"];
            String contentContact = Request["contentContact"];
            try
            {
                string conTent = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/newcontact.html"));
                conTent = conTent.Replace("{{UsernameContact}}", usernameContact);
                conTent = conTent.Replace("{{EmailContact}}", emailContact);
                conTent = conTent.Replace("{{SubjectContact}}", subjectContact);
                conTent = conTent.Replace("{{ContentContact}}", contentContact);
                var toEmailContact = ConfigurationManager.AppSettings["UsernameContact"].ToString();

                new Common.MailHelper().MailContact(emailContact, toEmailContact, "Liên lạc mới từ khách hàng", conTent);
                return View("SendSuccess");
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


    }
}