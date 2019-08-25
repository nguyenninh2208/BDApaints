function changeStatusFreeship(id) {
    event.preventDefault();
    swal({
        title: "Bạn chắc chắn thay đổi ?",
        text: "Trạng thái này sẽ được thay đổi",
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
                    url: "/SysAdmin/MemberShip_Promotion/changeStatusFreeship",
                    data: { id: id },
                    dataType: 'json',
                    success: function (rs) {
                        if (rs.toString() == "1") {
                            $("#btnChangeStatus_" + id).text('Có');
                            swal("Thay đổi thành công", "Nhấn OK để tiếp tục", "success");
                        }
                        else if (rs.toString()=="0") {
                            $("#btnChangeStatus_" + id).text('Không');
                            swal("Thay đổi thành công", "Nhấn OK để tiếp tục", "success");
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