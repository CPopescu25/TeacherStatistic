function editEmployee(e) {
    var id = $(e).data("id");
    window.open("/User/Edit/" + id);

}