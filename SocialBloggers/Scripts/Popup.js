$(document).ready(function () {
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
});