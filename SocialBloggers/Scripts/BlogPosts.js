var editid = 0;
$(document).ready(function () {
    $('.line input').keyup(function (event) {
        if (event.keyCode === 13) {
            var postid = $(this).parent().parent().parent().parent().attr('id');
            var message = $(this).val();
            message = message.replace(/\'/g, "''");
            $.ajax({
                type: 'POST',
                url: '/Home/Comment?postid=' + postid + "&message=" + message,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                statusCode: {
                    200: function () {
                        window.location.reload();
                    }
                }
            });
        }
        
    })
    $('.new-btn').click(function () {
        $('#newPost').toggleClass('collapsible');
    });
    $('.comment-btn').click(function () {
        var postid = $(this).parent().parent().attr('id');
        $('#' + postid + ' #comments .comments').toggleClass('collapsible');
    });
    $("#newpost-submit").click(function () {
        var title = $("#title").val();
        var content = $("#content").val();
        content = content.replace(/\'/g, "''");
        $.ajax({
            type: 'POST',
            url: '/Home/AddPost?title=' + title + '&content=' + content,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            statusCode: {
                200: function () {
                    window.location.reload();
                }
            }
        });
    });
    $("#login").click(function () {
        var username = $("#username").val();
        var password = $("#password").val();
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
    $('a.delete-btn').click(function () {
        var button = $(this);
        var commentID = button.attr('id');
       
        $.ajax({
            type: 'POST',
            url: '/Home/DeleteComment?commentId=' + commentID,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            statusCode: {
                200: function () {
                     button.parent().parent().toggle();
                }
            }
        });
    });
    $("#postBox .delete-btn").click(function () {
        var postid = $(this).parent().parent().attr('id');
        $.ajax({
            type: 'POST',
            url: '/Home/DeletePost?postid=' + postid,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            statusCode: {
                200: function () {
                    window.location.reload();
                }
            }
        });
    })
    $("button#unlike").click(function () {
        var postid = $(this).parent().parent().attr('id');
            $.ajax({
                type: 'POST',
                url: '/Home/UnlikePost?postid=' + postid,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                statusCode: {
                    200: function () {
                        window.location.reload();
                    }
                }
            });
    });
    $("button#like").click(function () {
        var postid = $(this).parent().parent().attr('id');
        $.ajax({
            type: 'POST',
            url: '/Home/LikePost?postid=' + postid,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            statusCode: {
                200: function () {
                    window.location.reload();
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
    $('#postBox .edit-btn').click(function () {
        $('#main').addClass('dim');
        $('#editModal').addClass('popup-open');
        var postid = $(this).parent().parent().attr('id');
        editid = postid;
        var content = $('#' + postid + ' #content').text();
        var title = $('#' + postid + ' #title').text();
        $('#editContent').val(content);
        $('#editTitle').val(title);
    });
    $('#save').click(function () {
        var content = $('#editContent').val();
        content = content.replace(/\'/g, "''");
        var title = $('#editTitle').val();
        var postid = editid;
        
        $.ajax({
            type: 'POST',
            url: '/Home/EditPost?postid=' + postid + "&title=" + title + "&content=" + content,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            statusCode: {
                200: function () {
                    window.location.reload();
                }
            }
        });
    });
    $("#close").click(function () {
        $('#main').removeClass('dim');
        $('#editModal').removeClass('popup-open');
    });
    $("#createAccount").click(function () {
        var form = $("form");
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            var username = $("#Username").val();
            var password = $("#Password").val();
            $.ajax({
                type: 'Post',
                url: '/Home/CreateAccount?username=' + username + '&password=' + password,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == -1) {

                    } else {
                        window.location = "/Home/Login/";
                    }
                }
            });
        }
    })
});