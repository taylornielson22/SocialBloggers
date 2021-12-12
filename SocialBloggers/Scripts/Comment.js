
$(document).ready(function () {
    $('.comment-btn').click(function () {
        var postid = $(this).parent().parent().attr('id');
        $('#' + postid + ' #comments .comments').toggleClass('collapsible');
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

    });
});
