﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;

    public partial class DELIVERY_ITEM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DELIVERY_ITEM()
        {
            this.CARTs = new HashSet<CART>();
        }
    
        public int DELIVERY_ITEM_ID { get; set; }
        public Nullable<int> DELIVERY_ID { get; set; }
        public Nullable<int> DELIVERY_TYPE_ID { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập giá vận chuyển")]
        public Nullable<int> DELIVERY_ITEM_COST { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập số ngày vận chuyển dự kiến")]
        public Nullable<int> ESTIMATED_DATE_DELIVERY { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên")]
        public string BOTH_NAME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CART> CARTs { get; set; }
        public virtual DELIVERY DELIVERY { get; set; }
        public virtual DELIVERY_TYPE DELIVERY_TYPE { get; set; }
    }
}
