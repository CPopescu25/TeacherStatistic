function exportS(e) {
    var id = $(e).data("id");
   
        $.ajax({
            url: "/Questionnaire/ExportByTeacherId",
            data: {id:id},
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
        });

     }
