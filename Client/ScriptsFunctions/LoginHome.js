var working = false;
$('.login').on('submit', function (e) {
   
    e.preventDefault();
    if (working) return;
    working = true;
    var $this = $(this),
      $state = $this.find('button > .state');
    $this.addClass('loading');
    $state.html('Authenticating');
    setTimeout(function () {
        $this.addClass('ok');
        $state.html('Welcome back!');
        setTimeout(function () {
            $state.html('Log in');
            $this.removeClass('ok loading');
            working = false;
        }, 4000);
    }, 3000);
    var model = {
        UserName: $('#username').val(),
        Password: $("#password").val()
    };

    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "/Home/LogIn",
        data: model,
        success: function (response) {
            if (response.Ok == true) {
                toastr.success(response.Message, { showDuration: 300 });
                location.replace("/Home/About");
            } else {
                toastr.error(response.Message, { showDuration: 300 });
            }

        },
    });
});