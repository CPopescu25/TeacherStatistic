function SaveChanges() {
    var Id = $("#Id").data("id");
    ///////PartialListUniversities
    var domains = new Array();
    $(".Universities").each(function (i, element) {
        var a = $(element).data("id");
        if (a != null) {
            domains.push({ "UniversityId": a, "UserId": Id });
        }
    });
    if (domains.length == 0) {
        var d = ($("#pd").text()).split(',');
        if (d != "") {
            for (var i = 0; i < d.length; i++) {
                domains.push({ "UniversityId": d[i], "UserId": Id });
            }
        } else {
            domains = 0;
        }
    }
    //PartialListGroup
    var groups = [];
    $(".Groups").each(function (i, element) {
        var a = $(element).data("id");
        if (a != null) {
            groups.push({ "GroupId": a, "UserId": Id });
        }
    });
   
    if (groups.length == 0) {
        var l = $(".Group").val();
        if (l != 0) {
            for (var i = 0; i < l.length; i++) {
                groups.push({ "GroupId": l[i], "UserId": Id });
            }
        } else {
            groups = 0;
        }
    }
    var y = $(".Years").data("id");
    if (y == null) {
        var y = $(".Year").val();
    }
    
    var datas = {
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        Email: $("#Email").val(),
        Phone: $("#Phone").val(),
        UserUniversity: domains,
        UserGroup: groups,
        Enable: true,
        UserName: $("#Username").val(),
        Id: Id,
        Year:y
    }
   
    $.ajax({
        url: "/User/Edit",
        data: { user: datas },
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