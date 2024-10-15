function editGroup(e) {
    var modalppe = document.getElementById("EditGroup");
    var spane = document.getElementsByClassName("close")[0];
    spane.onclick = function () {
        modalppe.style.display = "none";
        $('.popup').html(" ");
    }
    var id = $(e).data("id");
    $.ajax({
        type: "GET",
        url: "/Groups/PartialEdit/" + id,
        success: function (result) {

            $(result).appendTo('.popup');

        },
    });

    $("#editGroup").click(function () {
        ////Select new rights from partial
        modalppe.style.display = "none";
        var Id = $("#partial").data("id");
        ////Rihts
        var rights = [];
        var elems = $(".ListElement");
        $(elems).each(function (i, e) {
            var Nume = $(e).find(".Nume");
            var c = $(e).find(".c");
            var r = $(e).find(".r");
            var u = $(e).find(".u");
            var d = $(e).find(".d");
            var l = $(e).find(".l");

            var key = $(Nume).data("name");
            var pairC = $(c).is(":checked");
            var pairR = $(r).is(":checked");
            var pairU = $(u).is(":checked");
            var pairD = $(d).is(":checked");
            var pairL = $(l).is(":checked");

            var Id_permission = $(c).data("value");
            rights.push({ "PermissionType": key, "Create": pairC, "Read": pairR, "Update": pairU, "Delete": pairD, "List": pairL, "GroupId": Id, "Id": Id_permission });

        });
        var dataedit = {
            Name: $(e).data("value"),
            Permissions: rights
        }
        $.ajax({
            url: "/Groups/Edit/" + id,
            data: dataedit,
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

        $('.popup').html(" ");
        location.reload(true);
    });
}