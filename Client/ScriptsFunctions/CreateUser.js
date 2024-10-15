function CreateEmployee() {
    var host = ((location.href).split('/')[2]).split(':')[1];
    if ($("#FirstName").val() == "") {
        $("#FirstName").css("border-color", "red");
        $("#FirstName").title = "Required!";
    } else if ($("#LastName").val() == "") {
        $("#LastName").css("border-color", "red");
        $("#LastName").title = "Required!";
    }
else if ($("#Email").val() == "") {
        $("#Email").css("border-color", "red");
        $("#Email").title = "Required!";
    }
    else if ($("#Phone").val() == "") {
        $("#Phone").css("border-color", "red");
        $("#Phone").title = "Required!";
    }
  else {
      /////PartialListUniversities
        var domains = [];
        var d = ($("#pd").text()).split(',');
        if (d == "") {
            toastr.error("University is empty");
        } else {
            for (var i = 0; i < d.length; i++) {
                domains.push({ "UniversityId": d[i] })
            }
        }
        ////PartialListGroup
        var groups = [];
        var g = $("#Group").val();
        for (var i = 0; i < g.length; i++) {
            groups.push({ "GroupId": g[i] });
        }

        //PartialYear
        var year = $("#Year option:selected").val();
       
        var datas = {
            FirstName: $("#FirstName").val(),
            LastName: $("#LastName").val(),
            Email: $("#Email").val(),
            Phone: $("#Phone").val(),
            UserUniversity: domains,
            UserGroup: groups,
            Enable: true,
            Year:year
        }
        $.ajax({
            url: "/User/Create",
            data: { data: datas, host: host },
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result.Ok == true) {
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

}