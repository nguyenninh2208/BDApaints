$(document).ready(function () {
    $("#btnSubmit").click(function () {
        var data = $("#formData").serialize();
        $.ajax({
            url: "/SysAdmin/Delivery_Type/edit",
            type: "POST",
            data: data,
            success: function (rs) {
                if (rs.toString() == "1") {
                    swal("Cập nhật thành công", "Nhấn OK để tiếp tục", "success");
                }
                else {
                    swal("Cập nhật thất bại", "Vui lòng kiểm tra lại dữ liệu nhập", "error");
                }
            }, error: function () {
                swal("Cập nhất thất bại", "Vui lòng kiểm tra lại dữ liệu nhập", "error");
            }
        })
    })
})