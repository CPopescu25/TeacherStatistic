$("#Image").click(function () {
    $("#txtUploadFile").css('visibility', 'visible');

});

function readURL(input) {
    var imgsrc = $("#Image").attr('src');
    var id = $("#Image").data("id");
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#Image").attr('src', e.target.result)
            $("#txtUploadFile").css('visibility', 'hidden');
        };
        reader.readAsDataURL(input.files[0]);
    }
    var img = $("#txtUploadFile").get(0).files[0];
    var formData = new FormData();
    formData.append("txtUploadFile", img);
    $.ajax({
        url: "/PartialViews/Upload/" + id,
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.Ok == true) {
                toastr.success(data.Message, { showDuration: 300 });
            } else {
                toastr.error(data.Message, { showDuration: 300 });
            }
        }
    });

}