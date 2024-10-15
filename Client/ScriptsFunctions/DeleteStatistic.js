function deleteStatistic(e) {
    var Id = $(e).data("id");
    swal({
        title: "Are you sure you want to delete this Statistic?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes!",
        cancelButtonText: "No,cancel!",
        closeOnCancel: false,
        closeOnConfirm: false
    },
      function (isConfirmed) {
          if (isConfirmed) {
              $.ajax({
                  type: "POST",
                  url: "/Statistic/Delete/" + Id,
                  dataType: "html",
                  success: function (result) {
                      if (result.Ok == true) {
                          toastr.success(result.Message, { showDuration: 300 });

                      } else if (result == "Error!") {
                          toastr.error(result.Message, { showDuration: 300 });
                      }

                  }
              });
          }
          else {
              swal("Cancelled", "You have pressed cancel!");
          }
      })
    location.reload(true);
}
