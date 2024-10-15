function deleteEmployee(e) {
    var Id = $(e).data("id");
    swal({
        title: "Are you sure you want to disable this User?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes!",
        cancelButtonText: "No,cancel!",
        closeOnCancel: false,
        closeOnConfirm: true
    },
      function (isConfirmed) {
          if (isConfirmed) {
              $.ajax({
                  type: "POST",
                  url: "/User/Delete/" + Id,
                  dataType: "html",
                  success: function (result) {
                      if (JSON.parse(result)["Ok"] == true) {
                          toastr.success(JSON.parse(result)["Message"], { showDuration: 300 });

                      } else {
                          toastr.error(JSON.parse(result)["Message"], { showDuration: 300 });
                      }
                  }
              });
          }
          else {
              swal("Cancelled", "You have pressed cancel!");
          }
          location.reload(true);
      });

}