
namespace BDApaint.Model
{
    using System;
    using System.Collections.Generic;
    public class RECENT_PRODUCT
    {
        public int PRODUCT_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string IMAGE { get; set; }
        public Nullable<int> PRICE { get; set; }
        public DateTime LAST_VISITED { get; set; }
        public Nullable<int> DISCOUNT_VALUE { get; set; }
        public Nullable<int> STATUS { get; set; }
    }
}