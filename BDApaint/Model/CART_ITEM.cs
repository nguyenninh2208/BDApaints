//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BDApaint.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CART_ITEM
    {
        public int CART_ITEM_ID { get; set; }
        public Nullable<int> PRODUCT_ID { get; set; }
        public Nullable<int> CART_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string IMAGE { get; set; }
        public Nullable<int> PRICE { get; set; }
        public Nullable<int> UNIT_PRICE { get; set; }
        public Nullable<int> QUANTITY { get; set; }
        public Nullable<int> TOTAL_MONEY { get; set; }
        public Nullable<int> DISCOUNT_VALUE { get; set; }
    
        public virtual CART CART { get; set; }
        public virtual PRODUCT PRODUCT { get; set; }
    }
}
