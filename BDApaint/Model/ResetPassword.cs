using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace BDApaint.Model
{
    using System;
    using System.Collections.Generic;
    public class ResetPassword
    {
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu mới", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và nhập lại mật khẩu mới không trùng nhập . Vui lòng nhập lại!")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }
    }
}