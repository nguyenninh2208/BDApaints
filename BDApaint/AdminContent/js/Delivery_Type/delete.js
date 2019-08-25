﻿    function ConfirmDelete(idPro) {
        event.preventDefault();
        swal({
            title: "Bạn chắc chắn xoá ?",
            text: "Loại vận chuyển này sẽ được xoá khi bạn nhấn nút Có",
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
                        url: '/SysAdmin/Delivery_Type/delete',
                        type: "GET",
                        data: { id: idPro },
                        success: function (rs) {
                            if (rs == 1) {
                                swal("Xoá thành công", "Nhấn OK để tiếp tục", "success");
                                $("#row_" + idPro).remove();
                            }
                            else {
                                swal("Xoá không thành công", "Loại này thuộc 1 loại vận chuyển đã có trong cơ sở dữ liệu", "error");
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            swal("Lỗi khi xoá", "Vui lòng thử lại", "error");
                        }
                    });
                }
                else {
                    swal("Đã huỷ", "Thông tin vẫn còn nguyên", "error")
                }
            });
    }

