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
    var id = ((location.href).split('=')[1]).split('&')[0];
    var setType = (location.href).split('=')[2];

    var datas = {
        UserName: $('#username').val(),
        Password: $("#password").val()
    }
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "/Answer/LogIn",
        data: { model: datas, id: id, setType: setType },
        success: function (data) {
            if (JSON.parse(data)["Ok"] == true) {
                toastr.success(data.Message, { showDuration: 300 });

                setTimeout(function () {
                    location.replace("/Answer/Answer?id=" + id + "&SetType=" + setType);
                }, 3000); //second message will appear 3 seconds later
                
            }
        },
    });
    location.reload(true);

});