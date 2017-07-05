$(function () {
    var userId = $('.btn-primary').attr('data-UserId');
    var imageId = $('.btn-primary').attr('data-ImageId');
    $.get('/home/diduserlike', {ImageId: imageId, UserId: userId}, function(result){
        console.log(result);
        if (result == false){
            $(".btn-primary").removeAttr("disabled");
        }
    })
    $('.btn-primary').on('click', function () {
        $.post('/home/likeImage', {UserId: userId, ImageId: imageId}, function(data)
        {
            $('#like').text(`Likes Count: ${data}`)
            $(".btn-primary").attr("disabled", 'true');
        })
    })
})