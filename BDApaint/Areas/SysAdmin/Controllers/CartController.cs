using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class CartController : Controller
    {
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        // GET: Administrator/Cart
        public ActionResult Index()
        {
            var model = Entities.CARTs.ToList();
            return View(model);
        }

        public ActionResult detail(int? id)
        {
            var lst = Entities.CART_ITEM.Where(x=>x.CART_ID == id).ToList();
            ViewBag.lst = lst;
            var model = Entities.CARTs.Find(id);
            return View(model);
        }
        public JsonResult changeStatus(int id)
        {
            var cart = Entities.CARTs.Find(id);
            int rs = (int)cart.STATUS;
            if (rs == -1 || rs == 1)
            {
                rs = -999;
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                cart.STATUS = 1;
                rs = (int)cart.STATUS;
                int row = updateProduct(cart.CART_ID);
                int totalUser = updateTotalUser((int)cart.CART_ID ,(int)cart.USER_ID);
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public int updateProduct(int idCart)
        {
            var detailCart = Entities.CART_ITEM.Where(x => x.CART_ID == idCart).ToList();
            foreach (var i in detailCart)
            {
                var product = Entities.PRODUCTs.Find(i.PRODUCT_ID);
                product.AMOUNT -= i.QUANTITY;
                if (product.SELL == null)
                {
                    product.SELL = i.QUANTITY;
                }
                else
                {
                    product.SELL += i.QUANTITY;
                }
            }
            int row =   Entities.SaveChanges();
            return row;
        }

        public int updateTotalUser(int idCart,int idUser)
        {
            var user = Entities.USERs.Find(idUser);
            var cart = Entities.CARTs.Find(idCart);
            int total = 0;
            if (user.TOTAL_TRANSACTION == null)
            {
                user.TOTAL_TRANSACTION = cart.TOTAL_MONEY;
                
            }
            else
            {
                user.TOTAL_TRANSACTION += cart.TOTAL_MONEY;
            }
            total = (int)user.TOTAL_TRANSACTION;
            int member = updateMemberShip(user.ID, total);
            int row = Entities.SaveChanges();
            return row;
        }

        public int updateMemberShip(int idUser,int total)
        {
            try
            {
                var user = Entities.USERs.Find(idUser);
                var list = Entities.MEMBERSHIP_TYPE.ToList();
                int row = 0;
                int check = -1;
                foreach (var i in list)
                {
                    if ((total) >= i.MIN_POINT * 1000000 && total <= i.MAX_POINT * 1000000)
                    {
                        check = i.ID;
                    }
                }
                user.MEMBERSHIP_TYPE_ID = check;
                row = Entities.SaveChanges();
                return row;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
           
        }
    }
}