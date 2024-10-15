function demo_delete() {
    var ref = $('#jstree_demo').jstree(true),
     sel = ref.get_selected();
    if (!sel.length) { return false; }
    ref.delete_node(sel);
    $.ajax({
        type: 'post',
        url: url + '/Universities/Delete/' + sel,
        success: function (result) {
            if (result.Ok == true) {
                toastr.success(result.Message, { showDuration: 300 });
            } else {
                swal({
                    title: result.Message,
                    type: "warning",
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK!",
                    closeOnConfirm: true
                },
              function (isConfirmed) {
                  if (isConfirmed) {
                  }
                  else {
                      swal("You have not pressed OK!");
                  }
              });
            }
        }
    });
};