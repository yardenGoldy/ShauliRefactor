function onClickCheckbox(selector) {
    if ($(selector).hasClass("hidden")) {
        $(selector).removeClass("hidden");
    }
    else {
        $(selector).addClass("hidden");
    }
}