//create new question
function createQSecond() {
    var msq = document.getElementById("CreateSQ");
    var spansq = document.getElementsByClassName("close")[0];
    spansq.onclick = function () {
        msq.style.display = "none";
        $('.popupSCreate').html(" ");
    }

    $.ajax({
        type: "GET",
        url: "/Questions/PartialCreate",
        success: function (result) {
            $(result).appendTo('.popupSCreate');

        },
    });
    $("#Screate").click(function () {
        var V1 = parseInt($("#FV1").val());
        var V2 = parseInt($("#FV2").val());
        var V3 = parseInt($("#FV3").val());
        var V4 = parseInt($("#FV4").val());
        var V5 = parseInt($("#FV5").val());
        var datas = {
            QuestionName: $("#FQName").val(),
            A1: $("#FA1").val(),
            A2: $("#FA2").val(),
            A3: $("#FA3").val(),
            A4: $("#FA4").val(),
            A5: $("#FA5").val(),
            V1: V1,
            V2: V2,
            V3: V3,
            V4: V4,
            V5: V5,
            SetType: 2

        }
        $.ajax({
            url: "/Questions/Create",
            data: datas,
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result.Ok==true) {
                    toastr.success(result.Message, { showDuration: 300 });
                    $("#intSecond").load("/Questions/PartialSecondIndex");
                   
                } else {
                    toastr.error(result.Message, { showDuration: 300 });
                }
            },
            error: function (a, b, c) {

            }
        });
        msq.style.display = "none";
     

    });
}