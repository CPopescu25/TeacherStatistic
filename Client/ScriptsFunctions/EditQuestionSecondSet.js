function editSQuestion(e) {
    var modalppes = document.getElementById("SEdit");
    var spanes = document.getElementsByClassName("closeEditQS")[0];
    spanes.onclick = function () {
        modalppes.style.display = "none";
        $('.popupEditSQ').html(" ");
    }
    var ids = $(e).data("id");
   
    $.ajax({
        type: "GET",
        url: "/Questions/PartialEdit/" + ids,
        success: function (result) {
            $(result).appendTo('.popupEditSQ');

        },
    });

    $("#Sedit").click(function () {
        var v1 = parseInt($("#Fv1").val());
        var v2 = parseInt($("#Fv2").val());
        var v3 = parseInt($("#Fv3").val());
        var v4 = parseInt($("#v4").val());
        var v5 = parseInt($("#v5").val());
        var datass = {
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
            Id: ids,
            SetType: 2
        }
        $.ajax({
            url: "/Questions/Edit/" + ids,
            data: datass,
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result.Ok == true) {
                    toastr.success(result.Message, { showDuration: 300 });
                    $("#intSecond").load("/Questions/PartialSecondIndex");
                } else {
                    toastr.error(result.Message, { showDuration: 300 });
                }

            },
            error: function (a, b, c) {

            }
        })
        modalppes.style.display = "none";
        $('.popupEditSQ').html(" ");
        
    });
}