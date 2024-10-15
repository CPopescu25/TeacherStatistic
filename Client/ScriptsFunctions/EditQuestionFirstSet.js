function editFQuestion(e) {
    var modalppe = document.getElementById("FEdit");
    var spane = document.getElementsByClassName("closeEditQF")[0];
    spane.onclick = function () {
        modalppe.style.display = "none";
        $('.popupEditFQ').html(" ");
    }
    var id = $(e).data("id");

    $.ajax({
        type: "GET",
        url: "/Questions/PartialEdit/" + id,
        success: function (result) {
            $(result).appendTo('.popupEditFQ');

        },
    });

    $("#Fedit").click(function () {
        var v1 = parseInt($("#Fv1").val());
        var v2 = parseInt($("#Fv2").val());
        var v3 = parseInt($("#Fv3").val());
        var v4 = parseInt($("#v4").val());
        var v5 = parseInt($("#v5").val());
        var datas = {
            QuestionName: $("#FName").val(),
            A1: $("#Fa1").val(),
            A2: $("#Fa2").val(),
            A3: $("#Fa3").val(),
            A4: $("#a4").val(),
            A5: $("#a5").val(),
            V1: v1,
            V2: v2,
            V3: v3,
            V4: v4,
            V5: v5,
            Id: id,
            SetType: 1
        }
        $.ajax({
            url: "/Questions/Edit/" + id,
            data: datas,
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result.Ok == true) {
                    toastr.success(result.Message, { showDuration: 300 });

                } else {
                    toastr.error(result.Message, { showDuration: 300 });
                }

            },
            error: function (a, b, c) {

            }
        })
        modalppe.style.display = "none";
        $('.popupEditFQ').html(" ");
        
    });
}