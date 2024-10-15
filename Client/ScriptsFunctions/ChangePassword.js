function ChangePassword() {
    var datas = {
        Username: $("#Username").val(),
        OldPassword: $("#Old").val(),
        NewPassword: $("#New").val()
    }
    $.ajax({
        url: "/User/ChangePassword",
        data: { data: datas },
        type: "POST",
        dataType: "json",
        success: function (result) {
            if (result.Ok == true) {
                toastr.success(result.Message, { showDuration: 300 });
                location.replace("/Home/Index");
            } else {
                toastr.error(result.Message, { showDuration: 300 });
            }

        },
        error: function (a, b, c) {

        }
    })
}