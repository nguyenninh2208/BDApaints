
$(document).ready(function () {
    $("#btnSubmit").click(function () {
        var data = $("#formData").serialize();
        $.ajax({
            url: "/SysAdmin/Delivery/create",
            type: "POST",
            data: data,
            success: function (rs) {
                if (rs == 1) {
                    swal("Thêm thành công", "Nhấn OK để tiếp tục", "success");
                }
                else {
                    swal("Thêm thất bại", "Vui lòng kiểm tra lại dữ liệu nhập", "error");
                }
            }, error: function () {
                swal("Thêm thất bại", "Vui lòng kiểm tra lại dữ liệu nhập", "error");
            }
        })
    })

})
