$(document).ready(function () {
    $("#SetType").on("change", function () {
        var setType = $("#SetType option:selected").val();
        if (setType !== null) {
            $.ajax({
                url: "/PartialViews/PartialMultiListQuestions",
                data: { setType: setType },
                type: "GET",
                success: function (result) {

                    $(result).appendTo("#partialQ");
                    $("#Label").css("visibility", "visible");
                    $("#btn").css("visibility", "visible");
                }
            });
        }
    });

});
$("#getAllQ").click(function () {
    $(this).data('clicked', true);
    $("#Questions option").attr('selected', 'selected');
    $("#Questions").select2({
    });
    $("#Questions").attr('disabled', true);
});


function CreateSurvey() {
    var host = ((location.href).split('/')[2]).split(':')[1];

    var users = [];
    var emails = [];
    var n = $("#Employees").val();

    for (var i = 0; i < n.length; i++) {
        var u = n[i].split(';');
        
        users.push({ "UserId": u[0] });
        emails.push({ "Email": u[1], "User_Name": u[2] });

    }
   
    var questions = [];

    var q = $("#Questions").val();
    for (var j = 0; j < q.length; j++) {
        questions.push({ "QuestionsId": q[j] });

    }

    var teacher=$("#Employee option:selected").val();
    var ts = teacher.split(';');
    var tid = ts[0];
    var tname = ts[1];

    var setType = $("#SetType option:selected").val();
    var datas = {
        UserId: tid,
        FirstStatisticQuestions: questions,
        StudentsTargets: users,
        SetType: setType,
        Emails: emails

    };

    //send email
    var Id = $("#Employee option:selected").val();
    var user = $("#Employee").data("type");

    $.ajax({
        url: "/Questionnaire/Create",
        data: { data: datas ,st:emails,user:tname,host:host},
        type: "POST",
        dataType: "json",
        success: function (result) {
            if (result.Ok === true) {
                toastr.success(result.Message, { showDuration: 300 });
                location.reload(true);
            } else {
                toastr.error(result.Message, { showDuration: 300 });
            }
        },
        error: function (a, b, c) {

        }
    });

}