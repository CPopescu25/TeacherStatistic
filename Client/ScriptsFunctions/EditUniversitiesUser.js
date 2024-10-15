function EditUniv(e) {
    /////Apare popup-ul cu selectul multiplu pentru editare grupuri
    var modalu = document.getElementById("EditUniv");
    var spanu = document.getElementsByClassName("closeEditUniv")[0];
    spanu.onclick = function () {
        modalu.style.display = "none";
        $('.popupEditUniv').html(" ");
    }
    var id = $(e).data("id");

    // Se aduce partialul de multiselect cu optiunile din profil deja selectate
    $.ajax({
        type: "GET",
        url: "/PartialViews/PartialListUniversity",
        success: function (result) {
            $(result).appendTo('.popupEditUniv');
            $("#domainSection").css("width", "90%");
            //se iau valorile din detalii din person domain
            $(".Universities").each(function (i, element) {
                var id = $(element).data("id");
                $(function () {
                    //se doreste ca nodurile cu id-urile de mai sus sa fie deja selectate
                    $("#jstree_Domenii").on('loaded.jstree', function () {
                        $("#jstree_Domenii li[role=treeitem]").each(function () {
                            $("#jstree_Domenii").jstree('select_node', id)
                        });
                    });
                });
            });
        },
    });
    modalu.style.display = "block";
    ////Select new domains from partial
    $("#editUniv").click(function () {
        var textul = "";
        //lista cu numele nodurilor selectate
        $(".domainsListElementCandidate").each(function (a, b) {
            textul += "<li>" + $(b).text() + "</li>";
           
        })
        var d = "<ul class='text-muted'style='overflow:scroll;max-height:400px;overflow-x:hidden;overflow-y:hidden;margin-left:-25px;'>" + textul + "</ul>";
        $("#profilU").replaceWith(d);
        modalu.style.display = "none";

    });
}