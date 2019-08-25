
function changeStatusDiscount(id) {
    event.preventDefault();
    swal({
        title: "Bạn chắc chắn thay đổi ?",
        text: "Trạng thái khuyến mãi này sẽ được thay đổi",
        type: "warning",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Có",
        cancelButtonText: "Không",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: "GET",
                    url: "/SysAdmin/Product_Discount/changeStatusDiscount",
                    data: { id: id },
                    dataType: 'json',
                    success: function (rs) {
                        if (rs.toString() == "1") {
                            swal("Thay đổi thành công", "Nhấn OK để tiếp tục", "success");
                            $("#btnChangeStatus_" + id).text('Kích hoạt');
                        }
                        else if (rs.toString() == "0") {
                            swal("Thay đổi thành công", "Nhấn OK để tiếp tục", "success");
                            $("#btnChangeStatus_" + id).text('Ngưng Kích hoạt');
                        }
                        else {
                            swal("Thay đổi không thành công", "Vui lòng kiểm tra lại", "error");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        swal("Lỗi ", "Vui lòng thử lại", "error");
                    }
                });
            }
            else {
                swal("Đã huỷ", "Thông tin vẫn còn nguyên", "error")
            }
        });
}
