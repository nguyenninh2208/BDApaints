using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;
using System.Data.Entity.Validation;

namespace BDApaint.Controllers
{
    public class ProductSearchController : Controller
    {
        private DATABASELVTN1Entities db = new DATABASELVTN1Entities();

        // GET: /ProductSearch/
        public ActionResult Index()
        {
            var producer = db.PRODUCERs.ToList();

            ViewBag.producer = producer;
            var productType = db.PRODUCT_TYPE.ToList();
            ViewBag.productType = productType;
            var productNet = db.PRODUCTs.Select(x => x.NET).Distinct().OrderBy(x => x.Value).ToList();
            ViewBag.productNet = productNet;
            //var productDate = db.PRODUCTs.Select(x => Convert.ToDateTime(x.DATE_OF_MANUFACTURE).Month).Distinct().ToList();
            //ViewBag.productDate = productDate;

            return View("Index");
        }



        [HttpGet]
        public ActionResult ResultSearch(string optionsMoney, int optionsProducer, int optionsProductType, int optionsNet, string optionsStatus, string opstionOrderBy)
        {
            if (opstionOrderBy == "tangdan")
            {
                var model = GetSearchingData(optionsMoney, optionsProducer, optionsProductType, optionsNet, optionsStatus);
                return Json(model.Select(x => new
                {
                    PRODUCT_ID = x.PRODUCT_ID,
                    PRODUCT_NAME = x.PRODUCT_NAME,
                    IMAGE = x.IMAGE,
                    PRICE = x.PRICE
                }).OrderBy(x => x.PRICE).ToList(), JsonRequestBehavior.AllowGet);
            }
            else if (opstionOrderBy == "giamdan")
            {
                var model = GetSearchingData(optionsMoney, optionsProducer, optionsProductType, optionsNet, optionsStatus);
                return Json(model.Select(x => new
                {
                    PRODUCT_ID = x.PRODUCT_ID,
                    PRODUCT_NAME = x.PRODUCT_NAME,
                    IMAGE = x.IMAGE,
                    PRICE = x.PRICE
                }).OrderByDescending(x => x.PRICE).ToList(), JsonRequestBehavior.AllowGet);
            }
            var z = GetSearchingData(optionsMoney, optionsProducer, optionsProductType, optionsNet, optionsStatus);
            return Json(z.Select(x => new
            {
                PRODUCT_ID = x.PRODUCT_ID,
                PRODUCT_NAME = x.PRODUCT_NAME,
                IMAGE = x.IMAGE,
                PRICE = x.PRICE
            }).ToList(), JsonRequestBehavior.AllowGet);
        }


        public List<PRODUCT> GetSearchingData(string optionsMoney, int optionsProducer, int optionsProductType, int optionsNet, string optionsStatus)
        {
            var lst = new List<PRODUCT>();
            if (optionsMoney == "nhohon500" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "conhang")
            {
                var filter = db.PRODUCTs.Where(s => s.PRICE.Value < 500000).
                    Where(s => s.PRODUCER.PRODUCER_ID == optionsProducer).
                    Where(s => s.PRODUCT_TYPE_ID == optionsProductType).
                    Where(s => s.NET == optionsNet).
                    Where(s => s.AMOUNT != 0).
                    ToList();
                lst.InsertRange(lst.Count(), filter);
            }
            else if (optionsMoney == "nhohon500" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "hethang")
            {
                var filterx = db.PRODUCTs.Where(s => s.PRICE.Value >= 500000).
                    Where(s => s.PRICE <= 1000000).Where(s => s.
                        PRODUCER_ID == optionsProducer).
                        Where(s => s.PRODUCT_TYPE_ID == optionsProductType).
                        Where(s => s.NET == optionsNet).
                        Where(s => s.AMOUNT == 0).
                        ToList();
                lst.InsertRange(lst.Count(), filterx);

            }

            else if (optionsMoney == "500den1tr" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "conhang")
            {
                var filterx = db.PRODUCTs.Where(s => s.PRICE.Value >= 500000).
                    Where(s => s.PRICE <= 1000000).Where(s => s.
                        PRODUCER_ID == optionsProducer).
                        Where(s => s.PRODUCT_TYPE_ID == optionsProductType).
                        Where(s => s.NET == optionsNet).
                        Where(s => s.AMOUNT != 0).
                        ToList();
                lst.InsertRange(lst.Count(), filterx);

            }
            else if (optionsMoney == "500den1tr" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "hethang")
            {
                var filterx = db.PRODUCTs.Where(s => s.PRICE.Value >= 500000).
                    Where(s => s.PRICE <= 1000000).Where(s => s.
                        PRODUCER_ID == optionsProducer).
                        Where(s => s.PRODUCT_TYPE_ID == optionsProductType).
                        Where(s => s.NET == optionsNet).
                        Where(s => s.AMOUNT == 0).
                        ToList();
                lst.InsertRange(lst.Count(), filterx);

            }
            else if (optionsMoney == "tren1tr" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "conhang")
            {
                var filtery = db.PRODUCTs.Where(s => s.PRICE.Value > 1000000)
                    .Where(s => s.PRODUCER_ID == optionsProducer)
                    .Where(s => s.PRODUCT_TYPE_ID == optionsProductType)
                    .Where(s => s.NET == optionsNet).
                    Where(s => s.AMOUNT != 0)
                    .ToList();
                lst.InsertRange(lst.Count(), filtery);
            }
            else if (optionsMoney == "tren1tr" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "hethang")
            {
                var filtery = db.PRODUCTs.Where(s => s.PRICE.Value > 1000000)
                    .Where(s => s.PRODUCER_ID == optionsProducer)
                    .Where(s => s.PRODUCT_TYPE_ID == optionsProductType)
                    .Where(s => s.NET == optionsNet).
                    Where(s => s.AMOUNT == 0)
                    .ToList();
                lst.InsertRange(lst.Count(), filtery);
            }
            else if (optionsMoney == "tatcagia" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "conhang")
            {
                var filtery = db.PRODUCTs.Where(s => s.PRODUCER.PRODUCER_ID == optionsProducer).
                    Where(s => s.PRODUCT_TYPE_ID == optionsProductType).
                    Where(s => s.NET == optionsNet).
                    Where(s => s.AMOUNT != 0)
                    .ToList();
                lst.InsertRange(lst.Count(), filtery);
            }
            else if (optionsMoney == "tatcagia" && (optionsProducer != 0) && (optionsProductType != 0) && (optionsNet != 0) && optionsStatus == "hethang")
            {
                var filtery = db.PRODUCTs.Where(s => s.PRODUCER.PRODUCER_ID == optionsProducer).
                    Where(s => s.PRODUCT_TYPE_ID == optionsProductType).
                    Where(s => s.NET == optionsNet).
                    Where(s => s.AMOUNT == 0)
                    .ToList();
                lst.InsertRange(lst.Count(), filtery);
            }
            return lst;
        }

        public ActionResult ProductReady()
        {
            try
            {
                var lstProduct = getProduct();
                return Json(lstProduct.Select(x => new
                {
                    PRODUCT_ID = x.PRODUCT_ID,
                    PRODUCT_NAME = x.PRODUCT_NAME,
                    IMAGE = x.IMAGE,
                    PRICE = x.PRICE,
                    STATUS_NEW = x.STATUS
                }).OrderBy(x =>x.PRODUCT_ID).Take(8).ToList(), JsonRequestBehavior.AllowGet);
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

        public List<PRODUCT> getProduct()
        {
            List<PRODUCT> lsp = db.PRODUCTs.ToList();
            return lsp;
        }







        /// <summary>
        /// trả về 2 controller để tránh lỗi 500
        /// </summary>
        /// <param name="id"></param>
        /// <returns>lấy producer sản phẩm</returns>
        public ActionResult getProducerMain(int? id)
        {
            var modell = getProducer(id);
            return Json(modell.Select(x => new
            {
                PRODUCT_ID = x.PRODUCT_ID,
                PRODUCT_NAME = x.PRODUCT_NAME,
                IMAGE = x.IMAGE,
                PRICE = x.PRICE
            }).OrderBy(x => x.PRICE).ToList(), JsonRequestBehavior.AllowGet);
        }


        public List<PRODUCT> getProducer(int? id)
        {
            //ViewBag.List_Type = Entities.PRODUCT_TYPE.ToList();
            //ViewBag.List_Producer = Entities.PRODUCERs.ToList();
            //ViewBag.ListProducer_1 = new List<PRODUCER_TYPE>(Entities.PRODUCER_TYPE.ToList());
            List<PRODUCT> lsp = db.PRODUCTs.Where(x => x.PRODUCER.PRODUCER_ID == id).ToList();

            return lsp;
        }



        /// <summary>
        /// trả về 2 controller để tránh lỗi 500
        /// </summary>
        /// <param name="id"></param>
        /// <returns>lấy loại sản phẩm</returns>
        public ActionResult getProductTypeMain(int? id)
        {
            var modelll = getProductType(id);
            return Json(modelll.Select(x => new
            {
                PRODUCT_ID = x.PRODUCT_ID,
                PRODUCT_NAME = x.PRODUCT_NAME,
                IMAGE = x.IMAGE,
                PRICE = x.PRICE
            }).OrderBy(x => x.PRICE).ToList(), JsonRequestBehavior.AllowGet);
        }

        public List<PRODUCT> getProductType(int? id)
        {
            List<PRODUCT> lsp = db.PRODUCTs.Where(x => x.PRODUCT_TYPE_ID == id).ToList();
            return lsp;
        }

        //lấy sp mới
        public ActionResult getProductNewMain()
        {
            var modelNew = getProductNew();
            return Json(modelNew.Select(x => new
            {
                PRODUCT_ID = x.PRODUCT_ID,
                PRODUCT_NAME = x.PRODUCT_NAME,
                IMAGE = x.IMAGE,
                PRICE = x.PRICE,
                STATUS= x.STATUS
            }).OrderBy(x => x.PRICE).ToList(), JsonRequestBehavior.AllowGet);
        }


        public List<PRODUCT> getProductNew()
        {
            List<PRODUCT> lsp = db.PRODUCTs.Where(x => x.STATUS == 1).ToList();
            return lsp;
        }



        //lấy sp khuyến mãi
        public ActionResult getProductSaleMain()
        {
            var modelSale = getProductSale();
            return Json(modelSale.Select(x => new
            {
                PRODUCT_ID = x.PRODUCT_ID,
                PRODUCT_NAME = x.PRODUCT_NAME,
                IMAGE = x.IMAGE,
                PRICE = x.PRICE,
                DISCOUNT_VALUE = x.PRODUCT_DISCOUNT.DISCOUNT_VALUE,
                STATUS= x.STATUS
            }).OrderBy(x => x.PRICE).ToList(), JsonRequestBehavior.AllowGet);
        }
        public List<PRODUCT> getProductSale()
        {
            List<PRODUCT> lsp = db.PRODUCTs.Where(x => x.DISCOUNT != null).ToList();
            return lsp;
        }


    }
}
