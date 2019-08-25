
$(document).ready(function () {
    $("#btnSubmit").click(function () {
        var data = $("#formData").serialize();
        debugger
        $.ajax({
            type: "POST",
            url: '/SysAdmin/Product_Discount/edit',
            data: data,
            success: function (rs) {
                if (rs == "success") {
                    swal("Cập nhật thành công", "click OK để tiếp tục", "success");
                }
                else if (rs == "dateIsvalid") {
                    swal("Thêm thất bại", "Ngày kết thúc không được nhỏ hơn ngày tạo hoặc hiện tại", "error");
                }
                else {
                    swal("Thêm thất bại", "Vui lòng kiểm tra dữ liệu nhập", "error");
                }
            }, error: function (rs) {
                swal("Lỗi", " kiểm tra lại thông tin nhập", "error");

            }
        })
    })
})