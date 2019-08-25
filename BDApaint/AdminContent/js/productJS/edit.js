$("#btnSubmit").click(function () {
    var data = $("#editProduct").serialize();
    debugger
    $.ajax({
        type: "POST",
        url: '/SysAdmin/Product/Edit',
        data: data,
        success: function (rs) {
            if (rs.toString() == "success") {
                alert("ahihi");
                window.location.href = "/SysAdmin/Home/Index"
            }
            else {
                alert(rs);
                swal("Error", "Update Failed", "error");
            }
        }, error: function (rs) {
            alert(rs)
            swal("Error", "Check your data input", "error");
        }
    })
})
   