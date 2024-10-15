//Actions for second set of questions
function deleteSQuestion(e) {
    var Id = $(e).data("id");
    swal({
        title: "Are you sure you want to delete this Question?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes!",
        cancelButtonText: "No,cancel!",
        closeOnCancel: true,
        closeOnConfirm: true
    },
      function (isConfirmed) {
          if (isConfirmed) {
              $.ajax({
                  type: "POST",
                  url: "/Questions/Delete/" + Id,
                  dataType: "html",
                  success: function (result) {
                      if (JSON.parse(result)["Ok"]==true) {
                          toastr.success(JSON.parse(result)["Message"], { showDuration: 300 });
                          $("#intSecond").load("/Questions/PartialSecondIndex");
                      } else {
                          toastr.error(JSON.parse(result)["Message"], { showDuration: 300 });
                      }
                  }
              });
          }
          else {
              swal("Cancelled", "You have pressed cancel!");
          }
          
      });

}