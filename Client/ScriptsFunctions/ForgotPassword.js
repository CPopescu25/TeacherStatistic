function ForgotPassword() {
    var host = ((location.href).split('/')[2]).split(':')[1];
    var datas = {
        Username: $("#Username").val(),
        Email: $("#Email").val()
    }
    $.ajax({
        url: "/User/ForgotPassword",
        data: { data: datas, host: host },
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