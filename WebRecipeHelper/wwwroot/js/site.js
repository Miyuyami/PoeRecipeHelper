// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

const minPage = 1;
page = 1;
function stashPagesCount() {
    return $("#stash-highlights").children().length;
}

$("#previousPageButton").click(function () {
    page = Math.max(minPage, page - 1);
    update();
})

$("#nextPageButton").click(function () {
    page = Math.min(stashPagesCount(), page + 1);
    update();
})

function update() {
    if (page > minPage) {
        $("#previousPageButton").prop("disabled", false);
    } else {
        $("#previousPageButton").prop("disabled", true);
    }

    if (page < stashPagesCount()) {
        $("#nextPageButton").prop("disabled", false);
    } else {
        $("#nextPageButton").prop("disabled", true);
    }

    $("#stash-highlights").children().hide();
    $("#highlight-page-" + page).show();
}

$(document).ready(function () {
    update();
})