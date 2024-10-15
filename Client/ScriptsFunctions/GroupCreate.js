$(document).ready(function () {
    $('#Create').click(function () {
        CreateGroup();
    });

});
function CreateGroup() {

    var name = $("#Name").val();
    
    var datas = {
        Name: name,
        Permissions: rights
    };
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "/Groups/Create",
        data: datas,
        success: function (result) {
            if (result.Validare === false) {
                swal({
                    title: result.Message,
                    type: "warning",
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK!",
                    closeOnConfirm: true
                },
    function (isConfirmed) {
        if (isConfirmed) {
            if (result.Message === "You are not authorized!") {
                location.reload(true);
            }
        }
        else {
            swal("You have not pressed OK!");
        }
    });
            } else {
                toastr.success(result.Message, { showDuration: 300 });
                if (result.Permissions.List === true) {
                    location.replace("Index");
                }
            }
        },
        error: function (error) {
           
        }
    });

}