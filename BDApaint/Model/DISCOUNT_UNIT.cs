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
    
    public partial class DISCOUNT_UNIT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DISCOUNT_UNIT()
        {
            this.MEMBERSHIP_TYPE = new HashSet<MEMBERSHIP_TYPE>();
            this.PRODUCT_DISCOUNT = new HashSet<PRODUCT_DISCOUNT>();
        }
    
        public int ID { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập mô tả ")]
        public string DESCRIPTION { get; set; }
        public bool CURRENCY { get; set; }
        public Nullable<bool> PERCENTAGE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEMBERSHIP_TYPE> MEMBERSHIP_TYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCT_DISCOUNT> PRODUCT_DISCOUNT { get; set; }
    }
}
