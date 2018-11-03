function getTopPosts() {
    $.ajax('/Post/GetTopPosts').then(function (data) {
        var topPostsSection = $('#topPosts');

        for (var post of data) {
            var postView = $('<a></a>').attr('href', '/Post/Details/' + post.id).html('<h4>' + post.title + '</h4>');
            postView = $('<div></div>').append(postView);
            topPostsSection.append(postView);
        }
    });
}

getTopPosts();