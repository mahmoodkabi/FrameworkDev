$(document).ready(function () {
    $.ajax({
        type: "post",
        url: '@Url.Action("getPersonelPossition", "Home")',
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            $("#profile__info").text(res);
        },
        error: function (e) {
        }
    });

    var zindex = 10;

    $("div.card").click(function (e) {

        var isShowing = false;

        if ($(this).hasClass("d-card-show")) {
            isShowing = true
        }

        if ($("div.dashboard-cards").hasClass("showing")) {

            $("div.card.d-card-show")
                .removeClass("d-card-show");

            if (isShowing) {

                $("div.dashboard-cards")
                    .removeClass("showing");
            } else {

            }

            zindex++;
        } else {

            $("div.dashboard-cards")
                .addClass("showing");
            $(this)
                .css({ zIndex: zindex })
                .addClass("d-card-show");

            zindex++;
        }
    });
});

$(function () {
    $('#slideshow img:gt(0)').hide();
    setInterval(function () {
        $('#slideshow :first-child')
            .fadeOut(1000)
            .next('img')
            .fadeIn(1000)
            .end()
            .appendTo('#slideshow');
    }, 3000);
});

$(function () {
    $('.toggle-btn').click(function () {
        $('.filter-btn').toggleClass('open');
    });

    $('.filter-btn a').click(function () {
        $('.filter-btn').removeClass('open');
    });
});

$('#all').click(function () {
    $('ul.tasks li').slideDown(300);
});

$('#one').click(function () {
    $('.tasks li:not(.one)').slideUp(300, function () {
        $('.one').slideDown(300);
    });
});

$('#two').click(function () {
    $('.tasks li:not(.two)').slideUp(300, function () {
        $('.two').slideDown(300);
    });
});
$('#three').click(function () {
    $('.tasks li:not(.three)').slideUp(300, function () {
        $('.three').slideDown(300);
    });
});

$('.card').click(function (arg) {
    var url = $(this).attr('defaultURL');
    var ssName = $(this).attr('ssName');
    LoadSubSystem(url, ssName);
});

function LoadSubSystem(url, ssName) {
    $.cookie("ssName", ssName);
    window.location.href = url;
}

$('#liUserProfile').click(function () {
    LoadSubSystem('/Account/MyProfile', 'Management');
});

$('.card.is-checked').click(function (arg) {
    $(this).removeClass('is-checked');
});

jQuery(document).ready(function () {
    $.cookie("ssName", "");

    function startTime() {
        var today = new Date();
        var h = today.getHours();
        var m = today.getMinutes();
        var s = today.getSeconds();

        m = checkTime(m);
        s = checkTime(s);
        document.getElementById("clock").innerHTML = h + ":" + m + ":" + s;
        t = setTimeout(function () { startTime() }, 500);
    }

    function checkTime(i) {
        var n = i;

        if (i < 10) {
            n = "0" + i;
        }

        return n;
    }

    startTime();
});

$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Management/Notification/GetCountNote",
        success: function (data) {
            $("#NoteCount").html(data);
        },
        error: function () {
            $("#NoteCount").html("?");
        }
    });
});

$("#toMeNote").click(function () {
    $.cookie("ssName", "StaffPortal");
});
