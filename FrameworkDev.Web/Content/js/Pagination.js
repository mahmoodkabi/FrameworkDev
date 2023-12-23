
function generatePage(parent, clickPage) {
    $("#" + parent + " .paginationfirst").html('');
    var paginationDiv = $("#" + parent + " .paginationfirst");

    var txthiddenClickPage = document.createElement("input");
    txthiddenClickPage.type = "hidden";
    txthiddenClickPage.id = parent + "hiddenClickPage";
    txthiddenClickPage.name = "currentPage";
    paginationDiv[0].appendChild(txthiddenClickPage);

    var txthiddenPageSize = document.createElement("input");
    txthiddenPageSize.type = "hidden";
    txthiddenPageSize.id = parent + "hiddenPageSize";
    txthiddenPageSize.name = "pageSize";
    paginationDiv[0].appendChild(txthiddenPageSize);


    var aTag = document.createElement("a");
    aTag.setAttribute('href', "javascript:;");
    aTag.setAttribute('data-page', "prev");
    aTag.innerText = ">";
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        $("#" + parent +".paginationfirst-wrapper").addClass(" testimonial-group");
        $("#" + parent +".paginationfirst").addClass(" row");
        $(aTag).addClass("prev page-numbers col-xs-2");
    }
    else {
        $("#" + parent +".paginationfirst-wrapper").removeClass(" testimonial-group");
        $("#" + parent +".paginationfirst").removeClass(" row");
        $(aTag).addClass("prev page-numbers");
    }


    paginationDiv[0].appendChild(aTag);

    var startPage = parseInt(clickPage) - 2;
    var endPage = parseInt(clickPage) + 3;
    if (startPage < 1) {
        startPage = 1;
        endPage = 5;
    }

    for (var i = startPage; i < endPage; i++) {
        aTag = document.createElement("a");
        aTag.setAttribute('href', "javascript:;");
        aTag.setAttribute('data-page', i);
        aTag.innerText = i;
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent))
            $(aTag).addClass("page-numbers col-xs-2");
        else
            $(aTag).addClass("page-numbers");
        paginationDiv[0].appendChild(aTag);
    }

    var aTag = document.createElement("a");
    aTag.setAttribute('href', "javascript:;");
    aTag.setAttribute('data-page', "next");
    aTag.innerText = "<";
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent))
        $(aTag).addClass("next page-numbers col-xs-2");
    else
        $(aTag).addClass("next page-numbers");
    paginationDiv[0].appendChild(aTag);

    var activePage = $("#" + parent + " .paginationfirst").find("[data-page='" + clickPage + "']");
    $("#" + parent + ' .paginationfirst a').removeClass('current');
    $(activePage).addClass("current");
}



function clickAhrefPagination(parent ,e) {

    //تشخیص صفحه جاری
    if ($("#" + parent +" .paginationfirst a.page-numbers.current") != null) {
        prevNextPage = $("#" + parent + " .paginationfirst a.page-numbers.current").attr("data-page");
        clickPage = e.currentTarget.attributes[1].value;

        if (prevNextPage == undefined)
            prevNextPage = 1;
    }
    else {
        prevNextPage = 1;
        clickPage = 1;
    }

    //فعال و غیر فعال کردن صفحات
    $("#" + parent + '.paginationfirst a').removeClass('current');
    if ($(e.currentTarget).attr("data-page") != "prev" && $(e.currentTarget).attr("data-page") != "next") {
        $(e.currentTarget).addClass("current");
    }
    else {
        var activePage;
        if ($(e.currentTarget).attr("data-page") == "prev") {
            prevNextPage = parseInt(prevNextPage) - 1;
            clickPage = prevNextPage;
            activePage = $("#" + parent +" .paginationfirst").find("[data-page='" + clickPage + "']").next();
        }
        else {
            prevNextPage = parseInt(prevNextPage) + 1;
            clickPage = prevNextPage;
            activePage = $("#" + parent +" .paginationfirst").find("[data-page='" + clickPage + "']").next();
        }
        //$(activePage).addClass("current");
    }

    //$("#" + parent + "#hiddenClickPage").val(parseInt(clickPage));
    //$("#" + parent + "#hiddenPageSize").val(10);
    //$(e.currentTarget).closest('form').submit();

    generatePage(parent, clickPage);

    return clickPage;
}