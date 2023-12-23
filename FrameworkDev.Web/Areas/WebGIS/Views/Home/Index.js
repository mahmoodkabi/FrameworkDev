var arrLayerAttribute = [];
var fullExtent;
var url = ""
var userLayerPointURL = "https://mahmoodkabi.ir/arcgis/rest/services/UserLayer/UserLayerPoint/MapServer/0";
var userLayerLineURL = "https://mahmoodkabi.ir/arcgis/rest/services/UserLayer/UserLayerLine/MapServer/0";
var userLayerPolygonURL = "https://mahmoodkabi.ir/arcgis/rest/services/UserLayer/UserLayerPolygon/MapServer/0";
var urlSearch = userLayerPointURL;
var CustomerPolygon = [];
var urlFeature = "";
var idNewFeature;
var actionType = "";
var totalBookMark = 0;
var totalSearchResult = 0;
var strSearchAllLayerAllFeature = "";

window.addEventListener('load', function () {
    $("#sidebar-layers").removeClass("open");
    mapCreation("", "divMap", urlSearch, function (res1) {


        //به دست آوردن فول اکستند پس از لود شدن نقشه
        fullExtent = {
            Xmax: map.extent.xmax,
            Ymax: map.extent.ymax,
            Xmin: map.extent.xmin,
            Ymin: map.extent.ymin,
            Wkid: map.extent.spatialReference.wkid,
            Extent: map.extent,
            Sacle: getScale(),
            CenterPoint: getCenterPoint().x + " " + getCenterPoint().y
        }

        //خواندن و اضافه کردن بوک مارک های کاربر به ویجت
        getBookMark("", 1, 10);

        //خواندن و اضافه کردن لایه های کاربر به ویجت
        getUserLayer("", 1, 10);
        generatePage("layerPagination", 1);

        //جستجو
        //getSearchResult("", 1, 10);
        generatePage("searchPagination", 1);


        ////treeViewMap(parentName = 'treeService', groupService = 'MetroProject');
        //createSearchtBox(10, 'divSearch', 'BUSSTATIOSN', 'marker', function (resSearch) {
        //});

        //ابزار رسم شکل بر روي نقشه
        createDivGraphgic("drawShape", "point");

        //فعال کردن ترسیم بر روی نقشه
        addGraphicToMap(function () {
            var ii = 0;

            for (i = 0; i < map.graphics.graphics.length; i++) {
                if (map.graphics.graphics[i].geometry.type == "polygon") {
                    CustomerPolygon[ii] = [];

                    for (j = 0; j < map.graphics.graphics[i].geometry.rings[0].length; j++) {
                        CustomerPolygon[ii][j] = map.graphics.graphics[i].geometry.rings[0][j]; // نقاط پليگان
                    }
                    ii++;
                }
            }

            var dd = CustomerPolygon;
        });
    });

    //ارائه اطلاعات عارضه انتخاب شده
    callMapClick = function (event) {
        if (cursorKind == "defulat")
            return;

        //بازگرداندن آدرس با توجه به نقطه كليك شده روي نقشه 
        if (cursorKind == "reverseGeoCoding") {
            $("#loading").addClass("is-loading");
            document.getElementById("liResultReverseGeoCoding").innerHTML = "";

            $.ajax({
                type: "post",
                data: JSON.stringify({ "xy": event.mapPoint.x + " " + event.mapPoint.y }),
                url: '/webgis/MapAPIGis/ReverseGeoCoding',
                contentType: "application/json; charset=utf-8",
                success: function (results) {
                    //cursorKind = "defulat";
                    //$('#map_layers').css('cursor', 'default'); 
                    $("#loading").removeClass("is-loading");
                    document.getElementById("liResultReverseGeoCoding").innerHTML = results;

                },
                error: function (e) {
                    $("#loading").removeClass("is-loading");
                }
            });
        }
        else {
            var extentGeom = pointToExtent(map, event.mapPoint, 20);
            var filteredGraphics = dojo.filter(map.graphics.graphics, function (graphic) {
                return extentGeom.contains(graphic.geometry);
            })

            if (event == null || event.graphic == null || event.graphic.geometry == null)
                return;

            var id = 0, type = "";
            var liAttributes = "";
            document.getElementById("attrLayer").innerHTML = "";

            $("#loading").addClass("is-loading");
            //cursorKind = "defulat";

            //فیچرهای عارضه
            //در صورتی که بتوانیم فیچرهای عارضه ر ا بخوانیم
            if (event.graphic.attributes != null) {
                clearMap();

                if (event.graphic.geometry.type == "polygon") {
                    id = event.graphic.attributes.UserLayerPolygonId;
                    type = "polygon";
                }
                else if (event.graphic.geometry.type == "polyline") {
                    id = event.graphic.attributes.UserLayerLineId;
                    type = "polyline";
                }
                else if (event.graphic.geometry.type == "point") {
                    id = event.graphic.attributes.UserLayerPointId;
                    type = "point";
                }

                $.ajax({
                    type: "POST",
                    url: '/WebGIS/MapAPIGis/GetFeatureLayer',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ 'id': id, "type": type }),
                    success: function (results) {
                        for (var i = 0; i < results.length; i++) {
                            liAttributes += "<li style='direction: ltr;'>" + results[i].AttributeName +
                                "<input type='text' readonly='true' id ='" + results[i].Id + "_" + results[i].AttributeName + "'  alt ='" + type + "~" + results[i].Id + "~" + results[i].AttributeName + "' value='" + results[i].AttributeValue + "' style='right: 145px; position: absolute; width: 140px; border: solid; padding: 4px; background-color: lightgray;'> " +
                                "</li>";
                        }

                        document.getElementById("attrLayer").innerHTML = liAttributes;

                        hideRightSidebar();
                        $("#sidebar-feachers").addClass("open");
                        $("#loading").removeClass("is-loading");

                    },
                    error: function (e) {

                    }
                });

            }
            //// تنها به نوع عارضه دسترسی داشته باشیم
            ////از طریق مختصات عارضه را پیدا می کنیم
            //else {
            //    var strSHAPE = "";
            //    if (event.graphic.geometry.type == "polygon") {
            //        type = "polygon";
            //    }
            //    else if (event.graphic.geometry.type == "polyline") {
            //        type = "polyline";
            //    }
            //    else if (event.graphic.geometry.type == "point") {
            //        type = "point";
            //        strSHAPE = event.graphic.geometry.x + " " + event.graphic.geometry.y;
            //    }

            //    $.ajax({
            //        type: "POST",
            //        url: '/WebGIS/MapAPIGis/GetUserLayerShapeWithXY',
            //        contentType: "application/json; charset=utf-8",
            //        data: JSON.stringify({ 'shapeType': type, "strSHAPE": strSHAPE }),
            //        success: function (results) {
            //            for (var i = 0; i < results.length; i++) {
            //                liAttributes += "<li style='direction: ltr;'>" + results[i].AttributeName +
            //                    "<input type='text' readonly='true' id ='" + results[i].Id + "_" + results[i].AttributeName + "'  alt ='" + type + "~" + results[i].Id + "~" + results[i].AttributeName + "' value='" + results[i].AttributeValue + "' style='right: 145px; position: absolute; width: 140px; border: solid; padding: 4px; background-color: lightgray;'> " +
            //                    "</li>";
            //            }

            //            document.getElementById("attrLayer").innerHTML = liAttributes;

            //            hideRightSidebar();
            //            $("#sidebar-feachers").addClass("open");
            //            $("#loading").removeClass("is-loading");

            //        },
            //        error: function (e) {

            //        }
            //    });

            //}
        }


       
    }
})


function pointToExtent(map, point, toleranceInPixel) {
    //calculate map coords represented per pixel
    var pixelWidth = map.extent.getWidth() / map.width;
    //calculate map coords for tolerance in pixel
    var toleraceInMapCoords = toleranceInPixel * pixelWidth;
    //calculate & return computed extent
    return new esri.geometry.Extent(point.x - toleraceInMapCoords,
        point.y - toleraceInMapCoords,
        point.x + toleraceInMapCoords,
        point.y + toleraceInMapCoords,
        map.spatialReference);
    //map.spatialReference);
}


function onchangetLayerName(e) {
    var optionSelected = $("#comLayers").val();
    var liAttributes = "";
    var properties = [];
    var objectIDValue = 0;
    var layerID = -1;

    document.getElementById("attrLayer").innerHTML = "";

    for (i = 0; i < arrLayerAttribute.length; i++) {
        //لايه ذخير شده در آريه را پيدا کن و ويزگي هاي آن را نمايش بده
        if (arrLayerAttribute[i][0].indexOf(optionSelected) != -1) {
            for (var key in arrLayerAttribute[i][1]) {
                if (arrLayerAttribute[i][1].hasOwnProperty(key) && typeof arrLayerAttribute[i][1][key] !== 'function') {
                    properties.push(key);

                    var keyText = ((arrLayerAttribute[i][1][key] == null || arrLayerAttribute[i][1][key].trim() == "") ? "null" : arrLayerAttribute[i][1][key]);
                    if (key.toUpperCase() == "OBJECTID") {
                        objectIDValue = keyText;
                        layerID = arrLayerAttribute[i][2];
                    }

                    liAttributes += "<li style='direction: ltr;'>" + key +
                        "<input type='text' readonly='true' id='" + key + "' alt='" + arrLayerAttribute[i][2] + "' value='" + keyText + "' style='right: 145px; position: absolute; width: 140px; border: solid; padding: 4px; background-color: lightgray;'> " +
                        "</li>";
                }
            }
        }
    }

    document.getElementById("attrLayer").innerHTML = liAttributes;

    //نمایش عارضه انتخاب شده روی نقشه
    showOnMap(urlSearch, 'OBJECTID', objectIDValue, layerID, true, 'all', function (res) {
    });
}

$(".contains-icon-library > a").click(function (event) {
    if ($("#main-topics").hasClass("open"))
        $("#main-topics").removeClass("open");
    else
        $("#main-topics").addClass("open");

    $("#sidebar-search").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
});

$(".contains-icon-search > a").click(function (event) {
    if ($("#sidebar-search").hasClass("open"))
        $("#sidebar-search").removeClass("open");
    else
        $("#sidebar-search").addClass("open");

    $("#main-topics").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
});

$(".contains-icon-layers > a").click(function (event) {
    if ($("#sidebar-layers").hasClass("open"))
        $("#sidebar-layers").removeClass("open");
    else
        $("#sidebar-layers").addClass("open");

    $("#main-topics").removeClass("open");
    $("#sidebar-search").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
});


$(".contains-icon-places > a").click(function (event) {
    if ($("#sidebar-bookmark").hasClass("open"))
        $("#sidebar-bookmark").removeClass("open");
    else
        $("#sidebar-bookmark").addClass("open");

    $("#main-topics").removeClass("open");
    $("#sidebar-search").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
});

$(".contains-icon-map > a").click(function (event) {
    if ($("#sidebar-maps").hasClass("open"))
        $("#sidebar-maps").removeClass("open");
    else
        $("sidebar-maps").addClass("open");

    $("#main-topics").removeClass("open");
    $("#sidebar-search").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
});

$(".contains-icon-features > a").click(function (event) {
    if ($("#sidebar-feachers").hasClass("open"))
        $("#sidebar-feachers").removeClass("open");
    else
        $("#sidebar-feachers").addClass("open");

    $("#main-topics").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-search").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
});


$(".contains-icon-GeoCoding > a").click(function (event) {
    if ($("#sidebar-geoCoding").hasClass("open"))
        $("#sidebar-geoCoding").removeClass("open");
    else
        $("#sidebar-geoCoding").addClass("open");

    $("#main-topics").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-search").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
});

$(".contains-icon-reverse-geocoding > a").click(function (event) {
    if ($("#sidebar-reverse-geoCoding").hasClass("open"))
        $("#sidebar-reverse-geoCoding").removeClass("open");
    else
        $("#sidebar-reverse-geoCoding").addClass("open");

    $("#main-topics").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-search").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
});



$(".close.close-btn").click(function (event) {
    $("#main-topics").removeClass("open");
});


$(".material-icons.back-button.back-search-type").click(function (event) {
    $(".sidebar-search-layer.sidebar-search-suggestions").css("left", "340px");
})


$("#plssearch").click(function (event) {
    $("#exampleName").html("آزادی")
    replaceSearch("BUSSTATIOSN")
});


$("#melksearch").click(function (event) {
    $("#exampleName").html("91")
    replaceSearch("BUSROUTES")
});

$("#nozasiSearch").click(function (event) {
    $("#exampleName").html("01-845631")
    replaceSearch("melk")
});


$("#codePostisearch").click(function (event) {
    $("#exampleName").html("8379186772")
    replaceSearch("PostalCode")
});


$("#latlongsearch").click(function (event) {
    $("#exampleName").html("536859 3592698")
    $("#txtLatLongSearch").show();
    $("#tagInput").remove();
    $("#ulSearchId").remove();
    $(".sidebar-search-layer.sidebar-search-suggestions").css("left", "0px");
    $(".sidebar-search-layer.sidebar-search-suggestions").css({ backgroundColor: '#F5F5F5' });

});

$("#txtLatLongSearch").on("keyup", function (event) {


    if (event.keyCode == "13") {

        //حذف کارکتر اسپیس
        var xy = $("#txtLatLongSearch").val().split(" ");
        // xy.forEach(element => {
        // 	var index = xy.indexOf(element);
        // 	if (index !== -1) xy.splice(index, 1);
        // });

        if (xy != null) {
            var x = xy[0];
            var y = xy[1];
            setCoordinate(x, y, function (res) {

            });
        }
    }
});



function replaceSearch(layerNmaes) {

    $("#txtLatLongSearch").hide();
    $("#tagInput").remove();
    $("#ulSearchId").remove();

    switch (layerNmaes) {
        case "BUSSTATIOSN":
            createSearchtBox(10, 'divSearch', layerNmaes, 'marker', function (resSearch) {

            });
            break;
        case "BUSROUTES":
            createSearchtBox(10, 'divSearch', layerNmaes, 'polygon', function (resSearch) {

            });
            break;
        default:
            createSearchtBox(10, 'divSearch', layerNmaes, 'polygon', function (resSearch) {

            });
            break;
    }


    $(".sidebar-search-layer.sidebar-search-suggestions").css("left", "0px");
    $(".sidebar-search-layer.sidebar-search-suggestions").css({ backgroundColor: '#F5F5F5' });
}



$(".zoom-in").on("click", function (e) {
    map.setZoom(-1 / 2);
});

$(".zoom-out").on("click", function (e) {
    map.setZoom(1 / 2);
});


$("#idTools").on("click", function (e) {
    if ($("#main-tools>ul").width() == 0 || $("#main-tools>ul").width() == null) {
        $("#main-tools>ul").css("width", "390px");
        $("#main-tools>ul").css("padding-right", "10px");
    }
    else {
        $("#main-tools>ul").css("width", "0px");
        $("#main-tools>ul").css("padding-right", "0px");
        $("#main-tools .options").css("display", "none");
    }
});


$(".tools li a").each(function (e) {
    $(this).bind('click', function () {
        //alert('User clicked on "foo."');
    });
});

//save bookmark to database
$("#saveBookMarkMap").on("click", saveBookMarkMap);

function saveBookMarkMap() {

    var itemsBookmark = {
        Xmax: map.extent.xmax,
        Ymax: map.extent.ymax,
        Xmin: map.extent.xmin,
        Ymin: map.extent.ymin,
        Wkid: map.extent.spatialReference.wkid,
        Name: $("#txtBookmarkName").val(),
        Description: $("#txtBookmarkDescription").val(),
        Extent: map.Extent,
        Sacle: getScale(),
        CenterPoint: getCenterPoint().x + " " + getCenterPoint().y
    }

    $.ajax({
        type: "POST",
        url: '/WebGIS/MapAPIGis/AddBookMark',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'itemsBookmark': itemsBookmark }),
        success: function (results) {
            $("#bookmark-feachers").hide();
            getBookMark("", 1, 10);
            Alert('fa fa-smile-o', 'موفق', results.Message, 'green', '', null);
        },
        error: function (e) {
            Alert('fa fa-frown-o', 'خطا', 'خطا در ثبت اطلاعات', 'red', '', null);
        }
    });
}


// read user bookmarks
function getBookMark(search, page, pageSize) {
    var itemBookmark = '';
    var paginationBookmark = '';
    var extentMap;

    document.getElementById("lstBookmark").innerHTML = "";

    $.ajax({
        type: "post",
        data: JSON.stringify({ "search": search, "page": page, "pageSize": pageSize }),
        url: '/webgis/MapAPIGis/GetBookMark',
        contentType: "application/json; charset=utf-8",
        success: function (results) {
            totalBookMark = results.Total;

            results.Data.forEach(function (item) {
                var id = "bookmark" + item.BookMarkID;

                extentMap = {
                    Xmax: item.Xmax,
                    Ymax: item.Ymax,
                    Xmin: item.Xmin,
                    Ymin: item.Ymin,
                    Wkid: item.Wkid,
                    Sacle: item.Sacle,
                    CenterPoint: item.CenterPoint,
                    Rings: item.Rings
                }
                var xy = item.CenterPoint.split(" ");

                //لیست بوک مارک
                itemBookmark += '<li class="selected visible bookmark" onclick=showBookmark(' + item.BookMarkID + ',' + xy[0] + ',' + xy[1] + ',' + item.Sacle + ',' + item.Rings + ')> ' +
                    '<span class = "col-sm-7 bookmark-span" style="font-size: large; font-family: \'IRANSans\';" id=' + id + '>' + item.Name + '</span> ' +
                    '<div class="place-item col-sm-5"> ' +
                    '<i class="material-icons clear" id="deleteItemBookMarkId" id-bookmark-delete=' + item.BookMarkID + ' title="حذف" >remove_circle</i> ' +
                    //'<span class="visibility"><i class="material-icons">visibility</i></span> '+
                    '<i class="material-icons expand expanded bookmark">keyboard_arrow_down</i> ' +
                    '</div> ' +
                    '<div class="place-config"> ' +
                    '<form action="places-dialog-update"> ' +
                    '<div class="td-control td-control-text"> ' +
                    '<p style="font-size: medium; font-family: \'IRANSans\';">' + item.Description + '</p> ' +
                    '</div> ' +
                    '</form> ' +
                    '</div> ' +
                    '</li> ';

            });

            ////پیجینگ بوک مارک
            for (var i = 0; i < results.Total / pageSize; i++) {
                if (i == 0)
                    paginationBookmark += '<li class="active"><a href="#">' + (i + 1) + '</a></li>'
                else
                    paginationBookmark += '<li><a href="#">' + (i + 1) + '</a></li>'
            }

            document.getElementById("lstBookmark").innerHTML = itemBookmark;
            document.getElementById("lstBookmarkPagination").innerHTML = paginationBookmark;
            $(".place-config").hide();

        },
        error: function (e) {
        }
    });
}

$(document).on("click", ".material-icons.expand.expanded.bookmark", function () {
    $(this).closest(".selected.visible.bookmark").find('.place-config').toggle();

    if ($(this).html() == "keyboard_arrow_down")
        $(this).html("keyboard_arrow_up");
    else
        $(this).html("keyboard_arrow_down");
});

var id_bookmark_delete = 0;
$(document).on("click", "#deleteItemBookMarkId", function () {
    id_bookmark_delete = $(this).attr("id-bookmark-delete")

    ConfirmAlert('ذخیره', 'آیا می خواهید بوک مارک حذف گردد؟', deleteBookmark, 'حذف');
});

function deleteBookmark() {
    $.ajax({
        type: "POST",
        url: '/WebGIS/MapAPIGis/DeleteBookMark',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'bookMarkId': id_bookmark_delete }),
        success: function (results) {
            $("#bookmark-feachers").hide();
            getBookMark("", 1, 10);
            Alert('fa fa-smile-o', 'موفق', results.Message, 'green', '', null);
        },
        error: function (e) {
            Alert('fa fa-frown-o', 'خطا', 'خطا در حذف اطلاعات', 'red', '', null);
        }
    });

    id_bookmark_delete = 0;
}


$(document).on("click", ".material-icons.expand.expanded.bookmark", function () {
    $(this).closest(".selected.visible.bookmark").find('.place-config').toggle();

    if ($(this).html() == "keyboard_arrow_down")
        $(this).html("keyboard_arrow_up");
    else
        $(this).html("keyboard_arrow_down");
});


function showBookmark(id, x, y, sacle, rings) {
    ////Convert cordinate to web mercator and then add to userGraphicsLayer
    //for (var i1 = 0; i1 < rings.rings.length; i1++) {
    //    for (var j1 = 0; j1 < rings.rings[i1].length; j1++) {
    //        var p1 = convertCordinate(rings.rings[i1][j1][0], rings.rings[i1][j1][1])
    //        rings.rings[i1][j1][0] = p1.x
    //        rings.rings[i1][j1][1] = p1.y
    //    }
    //}

    addRings(rings);
    setCoordinate(x, y, function () {
        setTimeout(function () {
            setScale(sacle);
        }, 500)
    });
}


$("#btnBookmark").on("click", function () {
    $("#bookmark-feachers").show();
});

$("#closeBookmarkDiv").on("click", function (e) {
    $("#bookmark-feachers").hide();
});

var cursorKind = "defulat";
$("#liIdentity").on("click", function () {
    $("#sidebar-feachers").addClass("open");
    if (cursorKind == "defulat") {
        //$('#map_layers').css({ 'cursor': 'url(' + webServerURL + '/images/print-map-preview.png), auto' });
        $('#map_layers').css({ 'cursor': 'url(' + '/Content/images/print-map-preview.png), auto' });
        cursorKind = "identity";
    }
    else {
        $('#map_layers').css('cursor', 'default');
        cursorKind = "defulat";
    }
});

$("#btnFullExtent").on("click", function () {


    // var extent = new esri.geometry.Extent({
    //     "xmin": fullExtent.Xmin, "ymin": fullExtent.Ymin, "xmax": fullExtent.Xmax, "ymax": fullExtent.Ymax,
    //     "spatialReference": { "wkid": 32639 }
    // });
    map.setExtent(fullExtent.Extent, true);


});


//ویرایش فیچر عارضه انتخاب شده
$("#liEdit").on("click", function () {
    $("#liEdit").css("display", "none")
    $("#liAdd").css("display", "none")
    $("#liDelete").css("display", "none")
    $("#liCancle").css("display", "inline-block")
    $("#liSave").css("display", "inline-block")

    $("#attrLayer input").each(function (index, data) {
        //$(this).removeAttr('readonly')
        $(this).prop('readonly', false);
        $(this).css("background-color", "white");
    });

    actionType = "Update";
    urlFeature = "/WebGIS/MapAPIGis/EditFeatures";
});

//اضافه کردن فیچر به عارضه انتخاب شده
$("#liAdd").on("click", function () {
    $("#liEdit").css("display", "none")
    $("#liAdd").css("display", "none")
    $("#liDelete").css("display", "none")
    $("#liCancle").css("display", "inline-block")
    $("#liSave").css("display", "inline-block")

    actionType = "Insert";
    urlFeature = "/WebGIS/MapAPIGis/AddFeatures";
    idNewFeature = Date.now();

    var liAttributes = "<li id='li" + idNewFeature + "' style='direction: ltr;'>" +
        "<input type='text' id='txtFeatureName" + idNewFeature + "'  style='right: 3px; position: absolute; width: 140px; border: solid; padding: 4px;'> " +
        "<input type='text' id='txtFeatureValue" + idNewFeature + "'  style='right: 145px; position: absolute; width: 140px; border: solid; padding: 4px;'> " +
        "</li>";

    var currAttlayer = document.getElementById("attrLayer").innerHTML;
    document.getElementById("attrLayer").innerHTML = liAttributes + currAttlayer;
});

//حذف کردن عارضه  و فیچرهای آن 
$("#liDelete").on("click", function () {
    $("#liEdit").css("display", "none")
    $("#liAdd").css("display", "none")
    $("#liDelete").css("display", "none")
    $("#liCancle").css("display", "inline-block")
    $("#liSave").css("display", "inline-block")

    actionType = "Delete";
    urlFeature = "/WebGIS/MapAPIGis/DeleteFeatures";
});


$("#liCancle").on("click", function () {
    $("#liEdit").css("display", "inline-block")
    $("#liAdd").css("display", "inline-block")
    $("#liDelete").css("display", "inline-block")
    $("#liCancle").css("display", "none")
    $("#liSave").css("display", "none")

    $("#attrLayer input").each(function (index, data) {
        $(this).prop('readonly', true)
        $(this).css("background-color", "lightgray");
    });
});

$("#liSave").on("click", function () {
    $("#loading").addClass("is-loading");

    //خواندن مقادیر اتریبیوت ها
    var arr = new Array();
    $("#attrLayer input").each(function (index, item) {
        // فیچرهای جدید که در زمان اینسرت ایجاد می شوند را در آرایه اینسرت نکنیم
        //تنها فیچرهای جاری
        if (item.alt.split('~')[0].trim() != "") {
            var collection = {
                'Type': item.alt.split('~')[0],
                'UserLayerShapeId': item.alt.split('~')[1],
                'FeatureName': item.alt.split('~')[2],
                'FeatureValue': $("#" + item.id).val(),
                'IsNewFeature': 'false'
            };
            arr.push(collection);
        }
    })

    //اگر اینسرت باشد اطلاعات اتریبوت که ایجاد کرده ایم را اضافه می کنیم
    if (actionType == "Insert") {
        var collection = {
            'Type': '',
            'UserLayerShapeId': '0',
            'FeatureName': $("#txtFeatureName" + idNewFeature).val(),
            'FeatureValue': $("#txtFeatureValue" + idNewFeature).val(),
            'IsNewFeature': 'true'
        };
        arr.push(collection);
    }

    //اینسرت، آپدیت و یا حذف  عارضه
    $.ajax({
        type: "POST",
        url: urlFeature,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'features': arr }),
        success: function (result) {
            if (result.Result == "OK") {
                $("#attrLayer input").each(function (index, data) {
                    $(this).prop('readonly', true)
                    $(this).css("background-color", "lightgray");
                });

                $("#liEdit").css("display", "inline-block")
                $("#liAdd").css("display", "inline-block")
                $("#liDelete").css("display", "inline-block")
                $("#liCancle").css("display", "none")
                $("#liSave").css("display", "none")

                //بازخوانی اطلاعات عارضه از دیتابیس
                var liAttributes = "";
                $.ajax({
                    type: "POST",
                    url: '/WebGIS/MapAPIGis/GetFeatureLayer',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ 'id': arr[0].UserLayerShapeId, "type": arr[0].Type }),
                    success: function (results) {
                        for (var i = 0; i < results.length; i++) {
                            liAttributes += "<li style='direction: ltr;'>" + results[i].AttributeName +
                                "<input type='text' readonly='true' id ='" + results[i].Id + "_" + results[i].AttributeName + "'  alt ='" + arr[0].Type + "~" + results[i].Id + "~" + results[i].AttributeName + "' value='" + results[i].AttributeValue + "' style='right: 145px; position: absolute; width: 140px; border: solid; padding: 4px; background-color: lightgray;'> " +
                                "</li>";
                        }

                        document.getElementById("attrLayer").innerHTML = liAttributes;
                        $("#loading").removeClass("is-loading");
                    },
                    error: function (e) {
                        $("#loading").removeClass("is-loading");
                    }
                });


                Alert('fa fa-smile-o', 'موفق', result.Message, 'green', '', null);

            } else {
                Alert('fa fa-frown-o', 'خطا', result.Message, 'red', '', null);
                $("#loading").removeClass("is-loading");
            }

        },
        error: function (e) {
            Alert('fa fa-frown-o', 'خطا', 'خطا در ثبت اطلاعات', 'red', '', null);
            $("#loading").removeClass("is-loading");
        }
    });

});


$(".close-tab").on("click", function () {
    hideRightSidebar();
});

function hideRightSidebar() {
    $("#main-topics").removeClass("open");
    $("#sidebar-search").removeClass("open");
    $("#sidebar-layers").removeClass("open");
    $("#sidebar-bookmark").removeClass("open");
    $("#sidebar-service").removeClass("open");
    $("#sidebar-maps").removeClass("open");
    $("#sidebar-feachers").removeClass("open");
    $("#sidebar-khadamat").removeClass("open");
    $("#sidebar-geoCoding").removeClass("open");
    $("#sidebar-reverse-geoCoding").removeClass("open");
}

$("#btnPrint").on("click", printMap);

function printMap(e) {
    return;
    printToolsEsri("چاپ وضع موجود نقشه", 1200, "MAP_ONLY", function (res) {

    });
}

$("#pushMenuId").on("click", function () {
    if ($("body").hasClass("sidebar-collapse")) {
        $("#main-tools").css("top", "57px");
        $("#main-tools").css("right", "235px");
    }
    else {
        $("#main-tools").css("top", "57px");
        $("#main-tools").css("right", "54px");
    }
});

$("#btnZoomnext").on("click", function () {
    navigationPreNextExtent("Next");
});

$("#btnZoomprev").on("click", function () {
    navigationPreNextExtent("Prev");
});

$("#btnDrawShape").on("click", function () {
    if ($("#main-tools .options").css("display") == "none")
        $("#main-tools .options").css("display", "block");
    else
        $("#main-tools .options").css("display", "none");
});



$("#main-login").on("click", function () {
    if (getCookie("FrameworkDevAuth") != "") {
        if ($("#main-login .dropdown").css("display") == "none")
            $("#main-login .dropdown").css("display", "block");
        else
            $("#main-login .dropdown").css("display", "none");
    }
    else {
        window.location.href = "/Account/Login";
    }
});


//حذف ترسیم های قبلی بر روی نقشه
// سپس ایجاد ترسیم جدید
$(document).on("click", "#draw-info i", function () {
    clearMap();
});


//ذخیره پلیگان رسم شده به عنوان بوک مارک
$("#liSaveBookMark").on("click", function () {
    ConfirmAlertWithTextBox('ذخیره', 'آیا می خواهید پلیگون رسم شده به عنوان بوک مارک ذخیره گردد؟', saveAsBookmark, 'ذخیره');
});

function saveAsBookmark() {

    var ringCustomer = {
        "rings":
        map.graphics.graphics[0].geometry.rings
        ,
        "spatialReference": {
            "wkid": wkid
        }
    };

    //var ringCustomer = {
    //    "rings": [[[5754005.395058381, 3852600.430297088], [5754116.4676151, 3852564.6004400826], [5754069.888800992, 3852503.689683173], [5753944.484301472, 3852506.0783403064], [5753907.460115899, 3852584.904025719], [5754005.395058381, 3852600.430297088]]],
    //    "spatialReference": { "wkid": 102100 }
    //};


    var itemsBookmark = {
        Xmax: map.extent.xmax,
        Ymax: map.extent.ymax,
        Xmin: map.extent.xmin,
        Ymin: map.extent.ymin,
        Wkid: map.extent.spatialReference.wkid,
        Name: $("#idConfirmAlertWithTextBox").val(),
        Description: $("#idConfirmAlertWithTextBox_Description").val(),
        Extent: map.Extent,
        Sacle: getScale(),
        CenterPoint: getCenterPoint().x + " " + getCenterPoint().y,
        Rings: JSON.stringify(ringCustomer)
    }

    $.ajax({
        type: "POST",
        url: '/WebGIS/MapAPIGis/AddBookMark',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'itemsBookmark': itemsBookmark }),
        success: function (results) {
            $("#bookmark-feachers").hide();
            getBookMark("", 1, 10);
            Alert('fa fa-smile-o', 'موفق', results.Message, 'green', '', null);
        },
        error: function (e) {
            Alert('fa fa-frown-o', 'خطا', 'خطا در ثبت اطلاعات', 'red', '', null);
        }
    });
}


$("#searchBookmard").on("keyup", function (e) {
    getBookMark($(this).val(), 1, 10);
});


//نمایش پنل آپلود فایل
$("#add-user-layer").on("click", function () {
    $("#upload").toggleClass("open");
});

$("#colse-user-upload").on("click", function () {
    $("#upload").toggleClass("open");
});


////$('#files').fileupload();
//$("#start-upload-file").on("click", function (e) {
//    addKMLToMap(e);
//});

$("#files").change(function () {
    $("#upload").toggleClass("open");
    $("#loading").addClass("is-loading");

    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var data = new FormData();

    for (var i = 0; i < files.length; i++) {
        data.append("File", files[i]);
    }

    ////Add the input element values
    //data.append("BookId", bookId1);
    //data.append("Title", jQuery("#title").val());

    var url = "/WebGIS/MapAPIGis/AddFile";


    $.ajax({
        type: "POST",
        url: url,
        contentType: false,
        processData: false,
        data: data,
        success: function (res) {
            if (res.Result == "OK") {
                Alert('fa fa-smile-o', 'موفق', res.Message, 'green', '', null);
            } else {
                Alert('fa fa-frown-o', 'خطا', res.Message, 'red', '', null);
            }

            $("#loading").removeClass("is-loading");
        },
        error: function (ex) {
            Alert('fa fa-frown-o', 'خطا', 'خطا در بارگذاری فایل', 'red', '', null);
            $("#loading").removeClass("is-loading");
        }
    });
});

//فعال کردن و غیر فعال کردن منوی سمت چپ
$("#nav-main-sidebar ul li").on("click", function (e) {
    $("#nav-main-sidebar ul li").removeClass("open");
    $(this).addClass("open");
})

//نمایش لایه کاربر برای سایر کاربران
$("#showFile").on("click", function (e) {
    if ($(this).is(':checked'))
        $("#lblAlertUpload").css("visibility", "hidden");
    else
        $("#lblAlertUpload").css("visibility", "visible");
});



// read user Layers
var totalUserLayer = 0;
function getUserLayer(search, page, pageSize) {
    var itemUserLayer = '';
    var paginationUserLayer = '';
    var extentMap;

    document.getElementById("lstUserLayer").innerHTML = "";

    $.ajax({
        type: "post",
        data: JSON.stringify({ "search": search, "page": page, "pageSize": pageSize }),
        url: '/webgis/MapAPIGis/GetUserLayer',
        contentType: "application/json; charset=utf-8",
        success: function (results) {
            totalUserLayer = results.Total;


            results.Data.forEach(function (item) {
                var id = "userLayer" + item.UserLayerId;

                //لیست لایه ها
                itemUserLayer += '<li class="selected visible UserLayer" style="cursor: initial;">' +
                    '<span class = "col-sm-7 bookmark-span" style="font-size: large; font-family: \'IRANSans\';" id=' + id + '>' + item.FileName + '</span> ' +
                    '<div class="place-item col-sm-5"> ' +
                    '<i class="material-icons clear" id="deleteLayerId" id-UserLayer-delete=' + item.UserLayerId + ' title="حذف" style="cursor: pointer;">remove_circle</i> ' +
                    '<span class="visibility"><i id="layer' + item.UserLayerId + '" class="material-icons" onclick=showUserLayer(' + item.UserLayerId + ',' + item.UserLayerLinesCount + ',' + item.UserLayerPointsCount + ',' + item.UserLayerPolygonsCount + ') style="cursor: pointer;">visibility</i></span> ' +
                    '<i class="material-icons expand expanded UserLayer" style="cursor: pointer;">keyboard_arrow_down</i> ' +
                    '</div> ' +
                    '<div class="place-config"> ' +
                    '<form action="places-dialog-update"> ' +
                    '<div class="td-control td-control-text"> ' +
                    '<p style="font-size: medium; font-family: \'IRANSans\';">' + item.Description + '</p> ' +
                    '</div> ' +
                    '</form> ' +
                    '</div> ' +
                    '</li> ';

            });
            document.getElementById("lstUserLayer").innerHTML = itemUserLayer;




            //////پیجینگ بوک مارک
            //for (var i = 0; i < results.Total / pageSize; i++) {
            //    if (i == 0)
            //        paginationUserLayer += '<li class="active"><a href="#">' + (i + 1) + '</a></li>'
            //    else
            //        paginationUserLayer += '<li><a href="#">' + (i + 1) + '</a></li>'
            //}

            //document.getElementById("lstUserLayer").innerHTML = itemUserLayer;
            //document.getElementById("lstUserLayerPagination").innerHTML = paginationUserLayer;
            //$(".place-config").hide();

        },
        error: function (e) {
        }
    });
}

$(document).on('click', "#layerPagination .paginationfirst-wrapper a", function (e) {
    var clickPage = clickAhrefPagination("layerPagination", e);
    getUserLayer("", clickPage, 10);
});


var id_UserLayer_delete = 0;
$(document).on("click", "#deleteLayerId", function () {
    id_UserLayer_delete = $(this).attr("id-UserLayer-delete")

    ConfirmAlert('ذخیره', 'آیا می خواهید لایه حذف گردد؟', deleteUserLayer, 'حذف');
});

function deleteUserLayer() {
    $.ajax({
        type: "POST",
        url: '/WebGIS/MapAPIGis/DeleteUserLayer',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'userLayerId': id_UserLayer_delete }),
        success: function (results) {
            $("#UserLayer-feachers").hide();
            getUserLayer("", 1, 10);
            Alert('fa fa-smile-o', 'موفق', results.Message, 'green', '', null);
        },
        error: function (e) {
            Alert('fa fa-frown-o', 'خطا', 'خطا در حذف اطلاعات', 'red', '', null);
        }
    });

    id_UserLayer_delete = 0;
}


$(document).on("click", ".material-icons.expand.expanded.UserLayer", function () {
    $(this).closest(".selected.visible.UserLayer").find('.place-config').toggle();

    if ($(this).html() == "keyboard_arrow_down")
        $(this).html("keyboard_arrow_up");
    else
        $(this).html("keyboard_arrow_down");
});


//نمایش لایه
function showUserLayer(id, userLayerLinesCount, userLayerPointsCount, userLayerPolygonsCount) {

    //تغییر رنگ آیکون چشم در پنل لایه ها
    //هنگام عدم نمایش
    if ($("#layer" + id).css('color') == "rgb(0, 141, 76)") {
        $("#layer" + id).css('color', '#636b7b');

        map.removeLayer(map.getLayer('userLayer' + id));
    }
    else { // هنگام نمایش
        $("#layer" + id).css('color', '#008d4c');

        $("#loading").addClass("is-loading");

        //نمایش عارضه ها بر روی نقشه
        if (userLayerPointsCount > 0) {

            //فیلتر بر اساس لایه انتخاب شده
            showOnMapByGraphicsLayer(userLayerPointURL, "UserLayerId_fk=" + id, "", false, id, function (res1) {
                $("#loading").removeClass("is-loading");
                ////زوم بر روی لایه انتخاب شده
                //setCoordinate(res1[0].geometry.x, res1[0].geometry.y, function () {
                //    $("#loading").removeClass("is-loading");
                //});
            });
        }

        if (userLayerLinesCount > 0) {

            //فیلتر بر اساس لایه انتخاب شده
            showOnMapByGraphicsLayer(userLayerLineURL, "UserLayerId_fk=" + id, "", false, id, function (res1) {
                $("#loading").removeClass("is-loading");
                ////زوم بر روی لایه انتخاب شده
                //setCoordinate(res1[0].geometry.paths[0][0][0], res1[0].geometry.paths[0][0][1], function () {
                //    $("#loading").removeClass("is-loading");
                //});
            });
        }

        if (userLayerPolygonsCount > 0) {

            //فیلتر بر اساس لایه انتخاب شده
            showOnMapByGraphicsLayer(userLayerPolygonURL, "UserLayerId_fk=" + id, "", false, id, function (res1) {
                $("#loading").removeClass("is-loading");
                ////زوم بر روی لایه انتخاب شده
                //setCoordinate(res1[0].geometry.rings[0][0][0], res1[0].geometry.rings[0][0][1], function () {
                //    $("#loading").removeClass("is-loading");
                //});
            });
        }
    }
}


$("#searchAllLayerAllFeature").on("keyup", function (e) {
    strSearchAllLayerAllFeature = $(this).val();
    getSearchResult($(this).val(), 1, 10);
});

// Seach on all attribute on all layer
function getSearchResult(search, page, pageSize) {
    if (search.trim() == '') {
        document.getElementById("lstSearch").innerHTML = "";
        return;
    }

    var itemSearch = '';
    var paginationSearch = '';

    document.getElementById("lstSearch").innerHTML = "";

    $.ajax({
        type: "post",
        data: JSON.stringify({ "search": search, "page": page, "pageSize": pageSize }),
        url: '/webgis/MapAPIGis/GetSearchResult',
        contentType: "application/json; charset=utf-8",
        success: function (results) {
            //totalSearchResult = results.Total;

            results.forEach(function (item) {
                var id = "Search" + item.SearchId;

                //لیست لایه ها
                itemSearch += '<li class="selected visible searchRes" onclick=showSearch("' + item.Url + '","' + item.Field + '","' + item.Value + '",' + '"0"' + ',"' + true + '","' + item.typeShow + '","' + item.UserLayerShapeId + '")> ' +
                    '<span class = "col-sm-7 bookmark-span" style="font-size: large; font-family: \'IRANSans\';">' + item.AttributeValue + '</span> ' +
                    '<div class="place-item col-sm-5"> ' +
                    '<i class="material-icons expand expanded searchRes" style="cursor: pointer;">keyboard_arrow_down</i> ' +
                    '</div> ' +
                    '<div class="place-config"> ' +
                    '<form action="places-dialog-update"> ' +
                    '<div class="td-control td-control-text"> ' +
                    '<p style="font-size: medium; font-family: \'IRANSans\';">' + item.AttributeName + '</p> ' +
                    '<p style="font-size: medium; font-family: \'IRANSans\';">' + item.Description + '</p> ' +
                    '</div> ' +
                    '</form> ' +
                    '</div> ' +
                    '</li> ';

            });
            document.getElementById("lstSearch").innerHTML = itemSearch;
        },
        error: function (e) {
        }
    });
}

$(document).on('click', "#searchPagination .paginationfirst-wrapper a", function (e) {
    var clickPage = clickAhrefPagination("searchPagination", e);
    getSearchResult(strSearchAllLayerAllFeature, clickPage, 10);
});

$(document).on("click", ".material-icons.expand.expanded.searchRes", function () {
    $(this).closest(".selected.visible.searchRes").find('.place-config').toggle();

    if ($(this).html() == "keyboard_arrow_down")
        $(this).html("keyboard_arrow_up");
    else
        $(this).html("keyboard_arrow_down");
});


function showSearch(url, field, value, layerId, isClear, typeShow, userLayerShapeId) {
    showOnMapByGraphicsLayerForSearch(url + "/0", field + "=" + value, "", true, userLayerShapeId, "marker", function (res1) {

    });

    //showOnMap(url, field, value, layerId, true, typeShow);
}

$("#liGeoCoding").on("click", function (e) {
    if ($("#txtCounty").val().trim() == '' && $("#txtCity").val().trim() == '') {
        Alert('fa fa-frown-o', 'پيام', 'نام شهر و يا شهرستان را وارد كنيد', 'red', '', null);
        return;
    }

    $("#loading").addClass("is-loading");

    $.ajax({
        type: "post",
        data: JSON.stringify({
            "county": $("#txtCounty").val(),
            "city": $("#txtCity").val(),
            "zone": $("#txtZone").val(),
            "mabar1": $("#txtMabar1").val(),
            "mabar2": $("#txtMabar2").val(),
            "mabar3": $("#txtMabar3").val(),
        }),
        url: '/webgis/MapAPIGis/GeoCoding',
        contentType: "application/json; charset=utf-8",
        success: function (results) {
            $("#loading").removeClass("is-loading");

            if (results.trim() != '') {
                setCoordinateWithMarker(results.split(' ')[0], results.split(' ')[1], function () {

                });
            }
            else {
                Alert('fa fa-frown-o', 'پيام', 'جستجو نتيجه اي در برنداشت', 'info', '', null);
            }
        },
        error: function (e) {
            $("#loading").removeClass("is-loading");
            Alert('fa fa-frown-o', 'پيام', 'جستجو نتيجه اي در برنداشت', 'info', '', null);
        }
    });
});


$("#liReversegeoCoding").on("click", function (e) {
    if (cursorKind == "defulat") {
        $('#map_layers').css({ 'cursor': 'url(' + '/Content/images/print-map-preview.png), auto' });
        cursorKind = "reverseGeoCoding";
    }
    else {
        $('#map_layers').css('cursor', 'default');
        cursorKind = "defulat";
    }

    //document.getElementById("liResultReverseGeoCoding").innerHTML = "";

    //$('#map_layers').css({ 'cursor': 'url(' + '/Content/images/print-map-preview.png), auto' });
    //cursorKind = "reverseGeoCoding";
});