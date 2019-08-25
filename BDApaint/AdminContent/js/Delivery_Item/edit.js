$(document).ready(function () {
    $("#btnSubmit").click(function () {
        var data = $("#formData").serialize();
        debugger
        $.ajax({
            url: "/SysAdmin/Delivery_Item/edit",
            type: "POST",
            data: data,
            success: function (rs) {
                if (rs == 1) {
                    window.location = "/sysadmin/delivery_item";
                    alert("Cập nhật thành công");
                }
                else {
                    alert("Cập nhật thất bại", "Vui lòng kiểm tra lại dữ liệu nhập", "error");
                }
            },
            error: function () {
                alert("Cập nhật thất bại", "Vui lòng kiểm tra lại dữ liệu nhập", "error");
            }
        })
    })
})

