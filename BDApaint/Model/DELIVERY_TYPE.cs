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
    public partial class DELIVERY_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DELIVERY_TYPE()
        {
            this.DELIVERY_ITEM = new HashSet<DELIVERY_ITEM>();
        }
    
        public int DELIVERY_TYPE_ID { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên loại vận chuyển")]
        public string DELIVERY_TYPE_NAME { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập mô tả")]
        public string DESCRIPTION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DELIVERY_ITEM> DELIVERY_ITEM { get; set; }
    }
}