$(document).ready(function () {
    $("#btnSubmit").click(function () {
        var data = $("#formData").serialize();
        debugger
        $.ajax({
            type: "POST",
            url: '/SysAdmin/Product/create',
            data: data,
            success: function (rs) {
                swal("Thêm thành công", "click OK để tiếp tục", "success");
            }, error: function (rs) {
                swal("Lỗi", " kiểm tra lại thông tin nhập", "error");

            }
        })
    })
})
   