//Function to select one answer only for a question
function get(e) {
    //get the id of the question
    var id = $(e).data("id");
    //create new array for all input checkboxes
    var ck = new Array();
    ck = document.getElementsByName("type");
    //for each input
    ck.forEach(function (i, el) {
        //get only the inputs for one question (inputs id = question id)
        if ($(i).data("id") == id) {
            //create new array
            var ans = new Array();
            //where push the inputs for a single question
            ans.push(i);
            //for each input
            ans.forEach(function (a, b) {
                //set first all checkboxes false
                a.checked = false;
            });
            //set true only the checkbox selected
            //only one checkbox can be selected for a question
            e.checked = true;
        }
    });
}
function SaveAnswers() {
    var selected = [];
    $("input:checkbox:checked").each(function () {
        if ($(this).is(":checked") == true) {
            selected.push({ "QuestionsId": $(this).data("id"), "answers": $(this).val(), "value": $(this).data("value") });
        }

    });
    var datas = {
        UserId: $("#Employee").data("id"),
        StatisticAnswers: selected,
        SetType: $("#Employee").data("name")
    }
    $.ajax({
        url: "/Answer/Answer",
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