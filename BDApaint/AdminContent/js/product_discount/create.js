$(document).ready(function () {
    $("#btnSubmit").click(function () {        
        var data = $("#formData").serialize();
        debugger
        $.ajax({
            type: "POST",
            url: '/SysAdmin/Product_Discount/create',
            data: data,
            success: function (rs) {
                if (rs == "success") {
                    window.location = "/Sysadmin/product_discount/index";
                    alert("Thên thành công");
                }
                else if (rs == "DateIsvalid") {
                    alert("Ngày không hợp lệ");
                }
                else {
                    alert("Lỗi dữ liệu");
                }
            }, error: function (rs) {
                alert("Lỗi");
            }
        })
    })
})
   