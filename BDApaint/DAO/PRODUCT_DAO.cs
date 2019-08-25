using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using BDApaint.Model;
using BDApaint.Controllers;
namespace BDApaint.DAO
{
    public class PRODUCT_DAO
    {
        static DATABASELVTN1Entities Entities = new DATABASELVTN1Entities();

        public static IEnumerable<PRODUCT> ListAllProduct(int Page, int PageSize)
        {
            return Entities.PRODUCTs.OrderBy(x => x.PRODUCT_NAME).ToPagedList(Page, PageSize);
        }
        public List<PRODUCER> ListProducer()
        {
            return Entities.PRODUCERs.OrderBy(x => x.PRODUCER_ID).ToList();
        }
        public bool checkProductInOrder(int id)
        {

            var order = Entities.CART_ITEM.Where(x => x.PRODUCT_ID == id).ToList();
            return order.Count == 0 ? true : false;
        }
        public bool checkProductInImport(int id)
        {
            var import = Entities.IMPORT_ORDER_DETAIL.Where(x => x.PRODUCT_ID == id).ToList();
            return import.Count == 0 ? true : false;
        }
        public bool checkComment(int id)
        {
            var import = Entities.COMMENTs.Where(x => x.Product_ID == id).ToList();
            return import.Count == 0 ? true : false;
        }
    }
}