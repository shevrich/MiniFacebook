$(function () {
    var pop;

    clearTableAndPopulate();

    $("#register").on('click', function () {
        $(".register-modal").modal();
    })

    $("#login").on('click', function () {
        $(".login-modal").modal();
    })

    function clearTableAndPopulate() {
        $(".popular tr:gt(0)").remove();
        $.get('/home/GetPopImages', function (data) {
            pop = data;
            data.forEach(function (image) {
                $(".popular").append(`<tr><td><a href="/home/ViewImage?Id=${image.Id}"><img src="/Images/${image.FileName}" height="100"/></a></td><td>${image.FirstName} ${image.LastName}</td>` +
                    `<td>${image.Views}</td><td>${new Date(parseInt(image.DateUploaded.substr(6))).toLocaleDateString()}</td></tr>`);
            })
        });
        $(".recent tr:gt(0)").remove();
        $.get('/home/getRecentImages', function (data) {
            data.forEach(function (image) {               
                $(".recent").append(`<tr><td><a href="/home/ViewImage?Id=${image.Id}"><img src="/Images/${image.FileName}" height="100"/></a></td><td>${image.FirstName} ${image.LastName}</td>` +
                    `<td>${image.Views}</td><td>${new Date(parseInt(image.DateUploaded.substr(6))).toLocaleDateString()}</td></tr>`);
            })
        });
        $(".liked tr:gt(0)").remove();
        $.get('/home/getLikedImages', function (data) {
            data.forEach(function (image) {
                $(".liked").append(`<tr><td><a href="/home/ViewImage?Id=${image.Id}"><img src="/Images/${image.FileName}" height="100"/></a></td><td>${image.FirstName} ${image.LastName}</td>` +
                    `<td>${image.Views}</td><td>${image.LikesCount}</td><td>${new Date(parseInt(image.DateUploaded.substr(6))).toLocaleDateString()}</td></tr>`);
            })
        });

        };
});
  