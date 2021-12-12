$(document).ready(function () {
   
    $('.new-btn').click(function () {
        $('#newPost').toggleClass('collapsible');
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
    
});