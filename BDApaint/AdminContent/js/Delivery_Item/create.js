$(document).ready(function () {
    $("#btnSubmit").click(function () {
        var data = $("#formData").serialize();
        debugger
        $.ajax({
            url: "/SysAdmin/Delivery_Item/create",
            type: "POST",
            data: data,
            success: function (rs) {
                if (rs == 1) {
                    window.location ="/sysadmin/delivery_item";
                    alert("Thêm thành công");
                }
                else {
                    alert("Thêm thất bại, vui lòng kiểm tra lại dữ liệu nhập");
                }
            }, error: function () {
                alert("Thêm thất bại, vui lòng kiểm tra lại dữ liệu nhập");
            }
        })
    })
})
