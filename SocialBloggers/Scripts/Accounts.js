$(document).ready(function () {
    $("#login").click(function () {
        var username = $("#username").val();
        var password = $("#password").val();
        $('#username-error').attr('hidden', true);
        $('#password-error').attr('hidden', true);
        $.ajax({
            type: 'GET',
            url: '/Home/LoginAttempt?username=' + username + '&password=' + password,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == 0) {
                    $('#username-error').attr('hidden', false);
                } else if (data == -1) {
                    $('#password-error').attr('hidden', false);
                } else {
                    window.location = "/Home/Posts/"
                }
            }
        });
    });
    $("a#username").click(function () {
        var username = $(this).text();
        window.location = '/Home/Posts?user=' + username;
    });
    $("a#myProfile").click(function () {
        var username = $(this).text();
        window.location = "/Home/Posts?user=current";

    });
    $(".follow-btn").click(function () {
        var unfollow = $(this).text() == "Unfollow";
        var username = $(this).prev('a').text();
        if (unfollow) {
            $.ajax({
                type: 'POST',
                url: '/Home/UnFollow?username=' + username,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                statusCode: {
                    200: function () {
                        window.location.reload();
                    }
                }
            });
        } else {
            $.ajax({
                type: 'POST',
                url: '/Home/Follow?username=' + username,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                statusCode: {
                    200: function () {
                        window.location.reload();
                    }
                }
            });
        }
    });
    $("#createAccount").click(function () {
            $('#userexists-error').attr('hidden', true);
            var username = $("#Username").val();
            var password = $("#Password").val();
            $.ajax({
                type: 'GET',
                url: '/Home/LoginAttempt?username=' + username + '&password=' + username,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == -1) {
                        $('#userexists-error').attr('hidden', false);
                    } else {
                        $.ajax({
                            type: 'Post',
                            url: '/Home/CreateAccount?username=' + username + '&password=' + password,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            statusCode: {
                                200: function () {
                                    window.location = "/Home/Login/"
                                }
                            }
                         });
                    }
                }
            });
    });
});