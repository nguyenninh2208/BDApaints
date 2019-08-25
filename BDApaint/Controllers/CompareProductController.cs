using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDApaint.Model;
using System.Data.Entity.Validation;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OfficeOpenXml.Style;
using System.Windows.Media;
using System.Drawing;


namespace BDApaint.Controllers
{
    public class CompareProductController : Controller
    {
        private DATABASELVTN1Entities db = new DATABASELVTN1Entities();
        //
        // GET: /CompareProduct/
        public ActionResult Index()
        {
            try
            {
                List<COMPARE> sosanh = Session["sosanh"] as List<COMPARE>;
              
                if (sosanh != null)
                {
                    @ViewBag.countSoSanh = sosanh.Count();
                    return View("Index", sosanh);

                }
                else
                    return View("ErrorCompare");
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
        public ActionResult ThemVaoSoSanh(int id)
        {
            List<COMPARE> sosanh;
            Model.PRODUCT sp = db.PRODUCTs.Find(id);
            var pro = db.PRODUCT_DISCOUNT.Where(x => x.ID == sp.DISCOUNT).SingleOrDefault();
            //neu chua co ses
            if (pro != null)
            {
                if (Session["sosanh"] == null)
                {
                    sosanh = new List<COMPARE>();


                    //them sp vao sess
                    COMPARE newItem = new COMPARE()
                    {
                        PRODUCT_ID = sp.PRODUCT_ID,
                        IMAGE = sp.IMAGE,
                        PRICE = sp.PRICE,
                        PRODUCT_NAME = sp.PRODUCT_NAME,
                        DATE_OF_MANUFACTURE = sp.DATE_OF_MANUFACTURE,
                        LIMITED_USE = sp.LIMITED_USE,
                        DESCRIPTION = sp.DESCRIPTION,
                        NET = sp.NET,
                        SPECIFICATION = sp.SPECIFICATION,
                        PRODUCER_NAME = sp.PRODUCER.PRODUCER_NAME,
                        PRODUCT_TYPE_NAME = sp.PRODUCT_TYPE.TYPE_NAME,
                        DISCOUNT_VALUE = sp.PRODUCT_DISCOUNT.DISCOUNT_VALUE
                    };//tao ra list compare moi
                    sosanh.Add(newItem);
                    Session["sosanh"] = sosanh;
                }
                //da ton tai ses
                else
                {
                    sosanh = Session["sosanh"] as List<COMPARE>;
                    if (sosanh.FirstOrDefault(m => m.PRODUCT_ID == id) == null) //ko co sp trong so sanh
                    {

                        COMPARE newItem = new COMPARE()
                        {
                            PRODUCT_ID = sp.PRODUCT_ID,
                            IMAGE = sp.IMAGE,
                            PRICE = sp.PRICE,
                            PRODUCT_NAME = sp.PRODUCT_NAME,
                            DATE_OF_MANUFACTURE = sp.DATE_OF_MANUFACTURE,
                            LIMITED_USE = sp.LIMITED_USE,
                            DESCRIPTION = sp.DESCRIPTION,
                            NET = sp.NET,
                            SPECIFICATION = sp.SPECIFICATION,
                            PRODUCER_NAME = sp.PRODUCER.PRODUCER_NAME,
                            PRODUCT_TYPE_NAME = sp.PRODUCT_TYPE.TYPE_NAME,
                            DISCOUNT_VALUE = sp.PRODUCT_DISCOUNT.DISCOUNT_VALUE

                        };
                        sosanh.Add(newItem);

                    }
                }
                var count = sosanh.Count();
                return Json(sosanh, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Session["sosanh"] == null)
                {
                    sosanh = new List<COMPARE>();


                    //them sp vao sess
                    COMPARE newItem = new COMPARE()
                    {
                        PRODUCT_ID = sp.PRODUCT_ID,
                        IMAGE = sp.IMAGE,
                        PRICE = sp.PRICE,
                        PRODUCT_NAME = sp.PRODUCT_NAME,
                        DATE_OF_MANUFACTURE = sp.DATE_OF_MANUFACTURE,
                        LIMITED_USE = sp.LIMITED_USE,
                        DESCRIPTION = sp.DESCRIPTION,
                        NET = sp.NET,
                        SPECIFICATION = sp.SPECIFICATION,
                        PRODUCER_NAME = sp.PRODUCER.PRODUCER_NAME,
                        PRODUCT_TYPE_NAME = sp.PRODUCT_TYPE.TYPE_NAME,
                        DISCOUNT_VALUE = 0
                    };//tao ra list compare moi
                    sosanh.Add(newItem);
                    Session["sosanh"] = sosanh;
                }
                //da ton tai ses
                else
                {
                    sosanh = Session["sosanh"] as List<COMPARE>;
                    if (sosanh.FirstOrDefault(m => m.PRODUCT_ID == id) == null) //ko co sp trong so sanh
                    {

                        COMPARE newItem = new COMPARE()
                        {
                            PRODUCT_ID = sp.PRODUCT_ID,
                            IMAGE = sp.IMAGE,
                            PRICE = sp.PRICE,
                            PRODUCT_NAME = sp.PRODUCT_NAME,
                            DATE_OF_MANUFACTURE = sp.DATE_OF_MANUFACTURE,
                            LIMITED_USE = sp.LIMITED_USE,
                            DESCRIPTION = sp.DESCRIPTION,
                            NET = sp.NET,
                            SPECIFICATION = sp.SPECIFICATION,
                            PRODUCER_NAME = sp.PRODUCER.PRODUCER_NAME,
                            PRODUCT_TYPE_NAME = sp.PRODUCT_TYPE.TYPE_NAME,
                            DISCOUNT_VALUE = 0

                        };
                        sosanh.Add(newItem);

                    }
                }
                var count = sosanh.Count();
                return Json(sosanh, JsonRequestBehavior.AllowGet);
            }
           
        }

        public ActionResult removeAllItemCompare()
        {
            Session["sosanh"] = null;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveCompareItem(int id)
        {
            List<COMPARE> sosanh;
            sosanh = Session["sosanh"] as List<COMPARE>;
            COMPARE itemDelete = sosanh.FirstOrDefault(m => m.PRODUCT_ID.Equals(id));
            if (itemDelete != null)
            {
                sosanh.Remove(itemDelete);
            }
            return Json(itemDelete, JsonRequestBehavior.AllowGet);
        }


        private List<COMPARE> ListCompare()
        {
            List<COMPARE> sosanh;
            sosanh = Session["sosanh"] as List<COMPARE>;
            return sosanh;
        }

        private Stream CreateExcelFile(Stream stream = null)
        {
            var list = ListCompare();
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                excelPackage.Workbook.Properties.Author = "Nguyễn Văn Ninh";
                excelPackage.Workbook.Properties.Title = "So sánh sản phẩm";
                excelPackage.Workbook.Properties.Comments = "Kết quả so sánh sản phẩm";
                excelPackage.Workbook.Worksheets.Add("First Sheet");
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                //workSheet.Cells[1, 1].LoadFromCollection(list, true, TableStyles.Dark9);
                BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        [HttpGet]
        public ActionResult ExportExcelCompare()
        {
            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelFile();
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //tạo save as
            Response.AddHeader("Content-Disposition", "attachment; filename=SoSanhSanPham.xlsx");
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }
        private void BindingFormatForExcel(ExcelWorksheet worksheet, List<COMPARE> listItems)
        {
            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 10;
            // Tự động xuống hàng khi text quá dài
            worksheet.Cells.Style.WrapText = true;
            // Tạo header
            worksheet.Cells[1, 1].Value = "Mã sản phẩm";
            worksheet.Cells[2, 1].Value = "Tên sản phẩm";
            worksheet.Cells[3, 1].Value = "Ngày sản xuất";
            worksheet.Cells[4, 1].Value = "Hạn sử dụng";
            worksheet.Cells[5, 1].Value = "Mô tả";
            worksheet.Cells[6, 1].Value = "Thể tích";
            worksheet.Cells[7, 1].Value = "Giá";
            worksheet.Cells[8, 1].Value = "Hãng sản xuất";
            worksheet.Cells[9, 1].Value = "Loại sản phẩm";

            // Đỗ dữ liệu từ list vào 
            for (int i = 0; i < listItems.Count; i++)
            {
                var item = listItems[i];
                worksheet.Cells[1, 2 + i].Value = item.PRODUCT_ID + 1;
                worksheet.Cells[2, 2 + i ].Value = item.PRODUCT_NAME;
                worksheet.Cells[3, 2 + i ].Value = item.DATE_OF_MANUFACTURE;
                worksheet.Cells[4, 2 + i].Value = item.LIMITED_USE;
                worksheet.Cells[5, 2 + i].Value = item.DESCRIPTION;
                worksheet.Cells[6, 2 + i].Value = item.NET;
                worksheet.Cells[7, 2 + i].Value = item.PRICE;
                worksheet.Cells[8, 2 + i].Value = item.PRODUCER_NAME;
                worksheet.Cells[9, 2 + i].Value = item.PRODUCT_TYPE_NAME;
            }
        }

    }
}
