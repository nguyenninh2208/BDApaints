using BDApaint.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.DAO;

namespace BDApaint.Areas.SysAdmin.Controllers
{
    public class Import_OrderController : BaseController
    {
        // GET: Import_Order
        DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();
        IMPORT_DAO dao = new IMPORT_DAO();

        public ActionResult Index()
        {
            var model = Entities.IMPORT_ORDER.ToList();
            return View(model);
        }
        public int CreateIDImport() // Cơ chế sinh mã
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.IMPORT_ORDER.Find(id) != null)
            {

                return CreateIDImport();

            }
            if (id < 1)
                return CreateIDImport();
            return id;
        }

       
        public int CreateIDdetail()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (Entities.IMPORT_ORDER_DETAIL.Find(id) != null)
            {

                return CreateIDdetail();

            }
            if (id < 1)
                return CreateIDdetail();
            return id;
        }

        public ActionResult detailImport(int id)
        {
            var model = Entities.IMPORT_ORDER_DETAIL.Where(x => x.IMPORT_ID == id).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createDetailView(IMPORT_ORDER imOrder)
        {
            
            IMPORT_ORDER ip = new IMPORT_ORDER();
            ip.ID = CreateIDImport();
            ip.SUPPLIER_ID = imOrder.SUPPLIER_ID;
            ip.DATE_IMPORT = DateTime.Now;
            ip.TOTAL = 0;
            Entities.IMPORT_ORDER.Add(ip);
            Entities.SaveChanges();
            ViewBag.id = ip.ID;
            var NameS = Entities.SUPPLIERs.Where(x => x.ID == imOrder.SUPPLIER_ID).Select(x=>x.NAME).SingleOrDefault();
            ViewBag.NameS = NameS;
            List<PRODUCT> lst = Entities.PRODUCTs.ToList();
            SelectList sl = new SelectList(lst, "PRODUCT_ID", "PRODUCT_NAME");
            ViewBag.sl = sl;
            return View("createDetail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createDetail(IMPORT_ORDER_DETAIL detail)
        {
            List<PRODUCT> lst = Entities.PRODUCTs.ToList();
            SelectList sl = new SelectList(lst, "PRODUCT_ID", "PRODUCT_NAME");
            ViewBag.sl = sl;
            IMPORT_ORDER_DETAIL dt = new IMPORT_ORDER_DETAIL();
            dt.ID = CreateIDImport();
            dt.IMPORT_ID = detail.IMPORT_ID;
            dt.PRODUCT_ID = detail.PRODUCT_ID;
            dt.IMPORT_AMOUNT = detail.IMPORT_AMOUNT;
            ViewBag.id = dt.IMPORT_ID;
            Entities.IMPORT_ORDER_DETAIL.Add(dt);
            var nameI = Entities.IMPORT_ORDER.Where(x => x.ID == detail.IMPORT_ID).Select(x => x).SingleOrDefault();
            ViewBag.NameS = Entities.SUPPLIERs.Where(x => x.ID == nameI.SUPPLIER_ID).Select(x => x.NAME).SingleOrDefault();
            ViewBag.NameS = ViewBag.NameS.ToUpper();
            int rowinserted =Entities.SaveChanges();
            ViewBag.rowinserted = rowinserted;

            return View();
        }

        [HttpGet]
        public ActionResult createImport()
        {
            var P = Entities.PRODUCTs.ToList();
            ViewBag.product = P;
            var S = Entities.SUPPLIERs.ToList();
            ViewBag.supplier = S;
            var U = Entities.UNIT_PRODUCT.ToList();
            ViewBag.unit = U;
            var H = Entities.PERSON_IMPORT.ToList();
            ViewBag.person = H;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createImport(IMPORT_ORDER_DETAIL detail)
        {
            var P = Entities.PRODUCTs.ToList();
            ViewBag.product = P;
            var U = Entities.UNIT_PRODUCT.ToList();
            ViewBag.listU = U;
           
            try {
                IMPORT_ORDER ip = new IMPORT_ORDER()
                {
                    ID = CreateIDImport(),
                    DATE_IMPORT = DateTime.Now,
                    SUPPLIER_ID = int.Parse(Request["SUPPLIER_ID"]),
                    PERSON_IMPORT_ID = int.Parse(Request["PERSON_IMPORT_ID"]),
                };
                IMPORT_ORDER_DETAIL iod = new IMPORT_ORDER_DETAIL()
                {
                    ID = CreateIDdetail(),
                    PRODUCT_ID = detail.PRODUCT_ID,
                    IMPORT_ID = ip.ID,
                    IMPORT_AMOUNT = detail.IMPORT_AMOUNT,
                    IMPORT_AMOUNT_REAL = detail.IMPORT_AMOUNT_REAL,
                    IMPORT_UNIT_ID = detail.IMPORT_UNIT_ID,
                    IMPORT_PRICE = detail.IMPORT_PRICE,
                    IMPORT_INTO_MONEY = detail.IMPORT_PRICE * detail.IMPORT_AMOUNT_REAL,
                };
                PRODUCT pro = Entities.PRODUCTs.Find(detail.PRODUCT_ID);
                if (pro.BUYING == null)
                {
                    pro.BUYING = iod.IMPORT_AMOUNT_REAL;
                }
                else
                {
                    pro.BUYING += iod.IMPORT_AMOUNT_REAL;
                }
                pro.AMOUNT += iod.IMPORT_AMOUNT_REAL;
                pro.STATUS = 1;
                ip.TOTAL = iod.IMPORT_INTO_MONEY;
                
                var UnitName = Entities.UNIT_PRODUCT.Find(iod.IMPORT_UNIT_ID).NAME.ToString();
                var SupName = Entities.SUPPLIERs.Find(ip.SUPPLIER_ID).NAME.ToString();
                var PersonName = Entities.PERSON_IMPORT.Find(ip.PERSON_IMPORT_ID).NAME.ToString();
                ViewBag.SupName = SupName;
                ViewBag.UnitName = UnitName;
                ViewBag.ID = ip.ID;
                ViewBag.PersonName = PersonName;
                ViewBag.TOTAL = ip.TOTAL;
                ViewBag.Unit = iod.IMPORT_UNIT_ID;
                //KHU LƯU DATA
                Entities.IMPORT_ORDER.Add(ip);
                Entities.IMPORT_ORDER_DETAIL.Add(iod);
                int row = Entities.SaveChanges();
                List<IMPORT_ORDER_DETAIL> lst = Entities.IMPORT_ORDER_DETAIL.Where(x => x.IMPORT_ID == ip.ID).ToList();
                if (row >= 1)
                {
                    setAlert("Nhập sản phẩm thành công", "success");
                    return View("createNextImport",lst);
                }
                else
                {
                    setAlert("Nhập sản phẩm không thành công, vui lòng kiểm tra dữ liệu nhập vào", "error");
                    return RedirectToAction("createImport");
                }
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
                setAlert("Nhập sản phẩm không thành công, vui lòng kiểm tra dữ liệu nhập vào", "error");
                return RedirectToAction("createImport");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createNextImport(IMPORT_ORDER_DETAIL detail)
        {
            var P = Entities.PRODUCTs.ToList();
            ViewBag.product = P;
            var U = Entities.UNIT_PRODUCT.ToList();
            ViewBag.listU = U;
            int ipID = int.Parse(Request["IPID"]);
            //int IntipID = int.Parse(ipID);
            IMPORT_ORDER ip = Entities.IMPORT_ORDER.Find(ipID);
            //khu của import_order_detail
            int proID = (int) detail.PRODUCT_ID;
            if (dao.checkProductInImport(ipID, proID))
            {
                var Detail = Entities.IMPORT_ORDER_DETAIL.Where(x => x.IMPORT_ID == ipID).ToList();
                
                IMPORT_ORDER_DETAIL ud = new IMPORT_ORDER_DETAIL();
                foreach(var y in Detail)
                {
                    if (y.PRODUCT_ID == proID)
                    {
                        ud = Entities.IMPORT_ORDER_DETAIL.Find(y.ID);
                    }
                }
                ud.PRODUCT_ID = detail.PRODUCT_ID;
                int udOld = (int)ud.IMPORT_AMOUNT_REAL;
                int priceOld = (int)ud.IMPORT_PRICE;
                ud.IMPORT_AMOUNT_REAL += detail.IMPORT_AMOUNT_REAL;
                ud.IMPORT_PRICE = detail.IMPORT_PRICE;
                ud.IMPORT_UNIT_ID = detail.IMPORT_UNIT_ID;
                ud.IMPORT_INTO_MONEY = detail.IMPORT_PRICE * ud.IMPORT_AMOUNT_REAL;
                int rowUpdate = Entities.SaveChanges();
                List<IMPORT_ORDER_DETAIL> lstUpdate = Entities.IMPORT_ORDER_DETAIL.Where(x => x.IMPORT_ID == ipID).ToList();

                PRODUCT pros = Entities.PRODUCTs.Find(detail.PRODUCT_ID);
                pros.AMOUNT += detail.IMPORT_AMOUNT_REAL;
                pros.BUYING += detail.IMPORT_AMOUNT_REAL;
                pros.STATUS = 1;

                //cập nhật tổng tiền của đơn hàng
                ip.TOTAL += ud.IMPORT_PRICE * ud.IMPORT_AMOUNT_REAL - udOld * priceOld;
                //Xong luôn nè

                //ID của SUPPLIER
                var SupNames = Entities.SUPPLIERs.Find(ip.SUPPLIER_ID).NAME.ToString();
                ViewBag.SupName = SupNames;

                //Cái này để hiển thị bên View thôi
                var NameUnits = Entities.UNIT_PRODUCT.Find(detail.IMPORT_UNIT_ID).NAME.ToString();
                ViewBag.NameUnit = NameUnits;

                var PersonNames = Entities.PERSON_IMPORT.Find(ip.PERSON_IMPORT_ID).NAME.ToString();
                ViewBag.PersonName = PersonNames;

                ViewBag.ID = ip.ID;
                ViewBag.TOTAL = ip.TOTAL;

                if (rowUpdate >= 1)
                {
                    setAlert("Sản phẩm đã có trong đơn hàng, đã cập nhật số lượng sản phẩm và tổng tiền nhập của đơn hàng", "success");
                    return View("createNextImport", lstUpdate);
                }
                else
                {
                    setAlert("Sản phẩm đã có trong đơn hàng, cập nhật đơn hàng thất bại, vui lòng kiểm tra lại dữ liệu nhập vào", "error");
                    return View("createNextImport", lstUpdate);
                }
            }

            IMPORT_ORDER_DETAIL iod = new IMPORT_ORDER_DETAIL() {
            ID = CreateIDdetail(),
            PRODUCT_ID = detail.PRODUCT_ID,
            IMPORT_ID = ipID,
            IMPORT_AMOUNT = detail.IMPORT_AMOUNT,
            IMPORT_AMOUNT_REAL = detail.IMPORT_AMOUNT_REAL,
            IMPORT_UNIT_ID = detail.IMPORT_UNIT_ID,
            IMPORT_PRICE = detail.IMPORT_PRICE,
            IMPORT_INTO_MONEY = detail.IMPORT_PRICE * detail.IMPORT_AMOUNT_REAL,
            };
            //Cập nhật số lượng sản phẩm sau khi nhập kho
            PRODUCT pro = Entities.PRODUCTs.Find(detail.PRODUCT_ID);
            pro.AMOUNT += iod.IMPORT_AMOUNT_REAL;
            pro.BUYING += iod.IMPORT_AMOUNT_REAL;
            pro.STATUS = 1;
            
            //cập nhật tổng tiền của đơn hàng
            ip.TOTAL += iod.IMPORT_INTO_MONEY;
            //Xong luôn nè

            //ID của SUPPLIER
            var SupName = Entities.SUPPLIERs.Find(ip.SUPPLIER_ID).NAME.ToString();
            ViewBag.SupName = SupName;

            //Cái này để hiển thị bên View thôi
            var NameUnit = Entities.UNIT_PRODUCT.Find(detail.IMPORT_UNIT_ID).NAME.ToString();
            ViewBag.NameUnit = NameUnit;
           
            var PersonName = Entities.PERSON_IMPORT.Find(ip.PERSON_IMPORT_ID).NAME.ToString();
            ViewBag.PersonName = PersonName;

            ViewBag.ID = ip.ID;
            ViewBag.TOTAL = ip.TOTAL;
            //Oke xong

            Entities.IMPORT_ORDER_DETAIL.Add(iod);
            int row = Entities.SaveChanges();
            List<IMPORT_ORDER_DETAIL> lst = Entities.IMPORT_ORDER_DETAIL.Where(model => model.IMPORT_ID == ip.ID).ToList();
            if (row >= 1)
            {
                setAlert("Nhập sản phẩm thành công", "success");
                return View("createNextImport", lst);
            }
            else
            {
                setAlert("Nhập sản phẩm không thành công, vui lòng kiểm tra dữ liệu nhập vào","error");
               return View("createNextImport",lst);
            }
        } 
    }
}