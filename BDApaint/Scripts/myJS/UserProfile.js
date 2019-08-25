function getProfileUser() {
    $.ajax({
        type: "POST",
        data: data,
        url: '/User/getUserProfile',
        success: function (result) {
            if (result.toString() = "success") {
            
            }    
                }
    })
}