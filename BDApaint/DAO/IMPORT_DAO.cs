using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.DAO
{
    public class IMPORT_DAO : Controller
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        // GET: IMPORT_DAO
        public ActionResult Index()
        {
            return View();
        }
        
        public bool checkProductInImport(int importID,int productID)
        {
            var lstDetail = Entities.IMPORT_ORDER_DETAIL.Where(x => x.IMPORT_ID == importID);
            foreach(var i in lstDetail)
            {
                if (i.PRODUCT_ID == productID)
                    return true;
            }
            return false;
        }
    }
}