using BDApaint.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Cart_ItemController : BaseController
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        // GET: Administrator/Cart_Item
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult detailCart(int? id)
        {
            var model = Entities.CART_ITEM.Where(x => x.CART_ID == id).ToList();
            return View(model);
        }
        public ActionResult getCartItembyIdProduct(int id)
        {
            List<CART_ITEM> ci = Entities.CART_ITEM.Where(X => X.PRODUCT_ID == id).ToList();
            return View(ci);
        }
    }
}