function ConfirmDelete(idPro) {
    event.preventDefault();
    swal({
        title: "Bạn chắc chắn xoá ?",
        text: "Khuyến mãi này sẽ được xoá khi bạn nhấn nút Có",
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
                    url: '/SysAdmin/MemberShip_Promotion/Delete',
                    type: "GET",
                    data: { id: idPro },
                    success: function () {
                        swal("Xoá thành công", "Nhấn OK để tiếp tục", "success");
                        $("#row_" + idPro).remove();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        swal("Lỗi khi xoá", "Vui lòng thử lại", "Lỗi khi xoá");
                    }
                });
            }
            else {
                swal("Đã huỷ", "Thông tin vẫn còn nguyên", "error")
            }
        });
}
