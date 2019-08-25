function changeStatusCart(idCart) {   
    event.preventDefault();
    swal({
        title: "Bạn chắc chắn thay đổi ?",
        text: "Trạng thái giao dịch này sẽ được thay đổi",
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
                    url: "/SysAdmin/Cart/changeStatus",
                    data: { id: idCart },
                    dataType: 'json',
                    success: function (rs) {
                        if (rs.toString() == "1") {
                            swal("Thay đổi thành công", "Nhấn OK để tiếp tục", "success");
                            $("#btnChangeStatus_" + idCart).text('Giao dịch thành công');
                        }
                        else {
                            swal("Thay đổi không thành công", "Giao dịch đã thành công hoặc đã bị huỷ bỏ", "error");
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



