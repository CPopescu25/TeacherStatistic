function EditGroup(e) {
    /////Apare popup-ul cu selectul multiplu pentru editare grupuri
    var modal = document.getElementById("EditGroup");
    var spane = document.getElementsByClassName("closeEditGroup")[0];
    spane.onclick = function () {
        modal.style.display = "none";
        $('.popupEditGroup').html(" ");
    }
    var id = $(e).data("id");

    $.ajax({
        type: "GET",
        url: "/PartialViews/PartialListGroup",
        success: function (result) {
            $("#Group").select2({});

            $(result).appendTo('.popupEditGroup');

        },
    });
    modal.style.display = "block";

    ////Select new categorys from partial
    $("#editGroup").click(function () {
        var textN = "";
        $("#Group option:selected").each(function (a, b) {
            textN += "<li>" + $(b).text() + "</li> ";
        });
        var n = "<ul class='text-muted'style='overflow:scroll;max-height:400px;overflow-x:hidden;overflow-y:hidden;margin-left:-25px;'>" + textN + "</ul>";
        $("#profilG").replaceWith(n);

        modal.style.display = "none";
    });
}