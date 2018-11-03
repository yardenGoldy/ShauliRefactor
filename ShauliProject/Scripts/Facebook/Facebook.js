window.fbAsyncInit = function () {
    FB.init({
        appId: '1764072496997170',
        cookie: true,
        xfbml: true,
        autoLogAppEvents: true,
        display: 'popup',
        status: true,
        version: 'v2.10'
    });

};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = 'https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.10&appId=1764072496997170';
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));


$('#shareBtn').click(function () {
    FB.ui({
        method: 'share',
        display: 'popup',
        quote: 'Cool blog',
        href: 'https://www.youtube.com/watch?v=lRIQDhlZ7co'
    }, function (response) { });
})

$('#shareBtn2').click(function () {
    FB.ui({
        method: 'share',
        display: 'popup',
        quote: 'Cool blog',
        href: 'https://www.youtube.com/watch?v=A-kIm6KfTXc'
    }, function (response) { });
})


