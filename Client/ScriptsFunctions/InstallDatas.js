function Install() {
    var wrap = $(".row");
    var divLoad = $('<div></div>').addClass("overlay")
        .css("width", "90%")
        .css("height", "90%")
        .css("position", "center");

    var spanLoad = $('<span></span>').addClass("fa fa-refresh fa-spin")
        .css("margin-left", "45%")
        .css("margin-top", "10%");
    divLoad.append(spanLoad);

    wrap.replaceWith(divLoad);

    $.ajax({
        url: "/Home/Install",
        type: "POST",
        success: function (result) {
            if (result.Ok === true) {
                toastr.success(result.Message, { showDuration: 300 });
                setTimeout(function () {
                    location.replace("/Home/Index");
                }, 3000); //second message will appear 3 seconds later
               
            } else {
                $(".overlay").css("display", "none");
                toastr.error(result.Message, { showDuration: 300 });
            }
        },
        error: function (a, b, c) {

        }
    });
}