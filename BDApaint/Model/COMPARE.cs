

namespace BDApaint.Model
{
    using System;
    using System.Collections.Generic;
    public class COMPARE
    {
        public int COMPARE_ID { get; set; }
        public Nullable<int> PRODUCT_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string IMAGE { get; set; }
        public Nullable<System.DateTime> DATE_OF_MANUFACTURE { get; set; }
        public Nullable<int> LIMITED_USE { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<int> NET { get; set; }
        public Nullable<int> PRICE { get; set; }
        public Nullable<int> DISCOUNT_VALUE { get; set; }
        public string SPECIFICATION { get; set; }

        public string PRODUCER_NAME { get; set; }

        public string PRODUCT_TYPE_NAME { get; set; }



    }
    
}