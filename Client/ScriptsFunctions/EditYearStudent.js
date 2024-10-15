function EditYear(e) {
    /////Apare popup-ul cu selectul  pentru editare anul de facultate/ master pentru student
    var modaly = document.getElementById("EditYear");
    var spany = document.getElementsByClassName("closeEditYear")[0];
    spany.onclick = function () {
        modaly.style.display = "none";
        $('.popupEditYear').html(" ");
    }
    var id = $(e).data("id");

    $.ajax({
        type: "GET",
        url: "/PartialViews/PartialYear",
        success: function (result) {
            
            $(result).appendTo('.popupEditYear');

        },
    });
    modaly.style.display = "block";

    ////Select new categorys from partial
    $("#editYear").click(function () {
        var textY = $("#Year option:selected").text();
        
        var y = "<ul class='text-muted'style='overflow:scroll;max-height:400px;overflow-x:hidden;overflow-y:hidden;margin-left:-25px;'>" + textY + "</ul>";
        $("#profilY").replaceWith(y);

        modaly.style.display = "none";
    });
}