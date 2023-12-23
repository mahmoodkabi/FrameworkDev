dojo.require("esri.map");
dojo.require("esri.dijit.OverviewMap");
dojo.require("esri.symbols.PictureMarkerSymbol");
dojo.require("esri.InfoTemplate");
dojo.require("esri.symbols.SimpleFillSymbol");
dojo.require("esri.symbols.SimpleLineSymbol");
dojo.require("esri.tasks.IdentifyTask");
dojo.require("esri.tasks.IdentifyParameters");
dojo.require("esri.dijit.Popup");
dojo.require("dojo._base.array");
dojo.require("dojo.dom-construct");
dojo.require("dojo.dom");
dojo.require("dojo.on");
dojo.require("dojo.query");
dojo.require("esri.tasks.find");
dojo.require("dgrid.Selection");
dojo.require("esri.geometry.Extent");
dojo.require("dijit.form.Button");
dojo.require("dojo.parser");
dojo.require("esri.dijit.Measurement");
dojo.require("esri.symbols.SimpleMarkerSymbol");
dojo.require("esri.symbols.PictureFillSymbol");
dojo.require("esri.symbols.CartographicLineSymbol");
dojo.require("esri.toolbars.draw");
dojo.require("esri.graphic");
dojo.require("esri.layers.OpenStreetMapLayer");
dojo.require("esri.dijit.Scalebar");
dojo.require("esri.geometry.webMercatorUtils");
dojo.require("esri.layers.FeatureLayer");
dojo.require("esri.dijit.Print");
dojo.require("esri.tasks.PrintTemplate");
dojo.require("esri.dijit.Bookmarks");
dojo.require("esri.request");
dojo.require("esri.config");
dojo.require("dijit.layout.BorderContainer");
dojo.require("dijit.layout.ContentPane");
dojo.require("esri.tasks.query");
dojo.require("esri.tasks.QueryTask");
dojo.require("dojo._base.Color");
dojo.require("esri.toolbars.edit");
dojo.require("dijit.Menu");
dojo.require("dijit.MenuItem");
dojo.require("esri.toolbars.navigation");
dojo.require("dijit.registry");
dojo.require("dijit.MenuSeparator");
dojo.require("esri.layers.KMLLayer");
dojo.require("dojo.domReady!");

var URL, URLSearch, map, findTask, findParams;
var UrlAPI;
var identifyTask, identifyParams;
var IsIdentifyButtonClick;
var measure;
var divName, divTop1Map;
var arrAutoCompelete = [];
var publicUrl, publicField, publicValue, publicLayerID, publicIsClearMap, publicTypeShow, mapFn, mapFn1, publicUserLayerId;
var globalLatitude, globalLongitude;
var PubElements, layer, tb;
// markerSymbol is used for point and multipoint, see http://raphaeljs.com/icons/#talkq for more examples
var markerSymbol, lineSymbol, fillSymbol;
var haveNotErorr = true;
//var wkid = 3857;
var wkid = 4326;
//var wkid = 32639;
//var wkid = 102100;
var navToolbar;



function addjavascript() {
    //var printJs = 'printJs';
    //if (!document.getElementById(printJs)) {
    //	var js = document.createElement('script');
    //	js.id = printJs;
    //	js.src = 'http://localhost/MapApi/printThis.js';
    //	document.head.appendChild(js);
    //}
}
function mapCreation(url, _divName, urlSearch, fn) {
    URL = url;
    URLSearch = urlSearch
    divName = _divName;
    mapFn = fn;

    createContanerMap();

    dojo.ready(mapCreationReady);
}





function mapCreationReady() {
    //map = new esri.Map("map");
    //var startExtent = new Extent(-95.271, 38.933, -95.228, 38.976,
    //    new SpatialReference({ wkid: wkid }));

    map = new esri.Map("map", {
        center: [51.6660, 32.6539],
        zoom: 14,
    });

    var URLs = URL.split(",");
    var layerbaseMapID = "";


    var openStreetMapLayer = new esri.layers.OpenStreetMapLayer({ id: "OSM" });
    map.addLayer(openStreetMapLayer);

    //map.centerAndZoom(new esri.geometry.Point(selectedTaxLot[0].geometry.x, selectedTaxLot[0].geometry.y, new esri.SpatialReference({ wkid: wkid })), 0.3);

    if (URL.trim() != "") {
        URLs.forEach(function (entry) {
            layerbaseMapID = entry.split('/');
            layer = new esri.layers.ArcGISDynamicMapServiceLayer(entry, { id: layerbaseMapID[layerbaseMapID.indexOf("MapServer") - 1] });
            map.addLayer(layer);
        });
    }



    dojo.parser.parse();

    //map.on("load", mapReady);
    mapReady();
    IsIdentifyButtonClick = false;


}

function mapReady() {
    esri.config.defaults.geometryService = new esri.tasks.GeometryService("http://webgis.ir:6080/arcgis/rest/services/Utilities/Geometry/GeometryServer");

    //map.setZoom(0.1);

    // markerSymbol is used for point and multipoint, see http://raphaeljs.com/icons/#talkq for more examples
    markerSymbol = new esri.symbol.SimpleMarkerSymbol();
    markerSymbol.setPath(webServerURL + '/Content/Images/marker.png');
    //markerSymbol.setColor(new esri.Color("#00FFFF"));

    map.on("click", mapClick);
    map.on("ExtentChange", mapExtentChange)

    //create identify tasks and setup parameters
    identifyTask = new esri.tasks.IdentifyTask(URLSearch);
    identifyParams = new esri.tasks.IdentifyParameters();
    identifyParams.tolerance = 20;
    identifyParams.returnGeometry = true;
    //identifyParams.layerIds = [7];
    identifyParams.layerOption = esri.tasks.IdentifyParameters.LAYER_OPTION_ALL;
    identifyParams.width = map.width;
    identifyParams.height = map.height;

    tb = new esri.toolbars.Draw(map);
    tb.on("draw-end", addGraphic);

    navToolbar = new esri.toolbars.Navigation(map);
    //dojo.on(navToolbar, "onExtentHistoryChange", extentHistoryChangeHandler);

    mapFn();
}


function createContanerMap() {
    divTop1Map = document.getElementById(divName);

    var div = document.createElement('div');
    divTop1Map.appendChild(div);
    div.id = 'map';

    // var div1 = document.createElement('div');
    // div.appendChild(div1);
    // div1.id = 'div1';

    // var divMap = document.createElement('div');
    // div1.appendChild(divMap);
    // divMap.id = 'map';


}

//***********************************************************
function createSearchtBox(take, parentElement, layerName, typeShow, fn) {

    mapFn1 = fn;
    var parEl = document.getElementById(parentElement)
    var tagInput = document.createElement('input');
    parEl.appendChild(tagInput);
    tagInput.id = 'tagInput';
    tagInput.name = 'tagInput';
    tagInput.placeholder = 'جستجو';
    tagInput.style = 'font-family:IRANSANS,FontAwesome; border-radius: 25px;padding:8px; text-align: center;';
    var att = document.createAttribute("list");
    att.value = "tagDatalist";
    tagInput.setAttributeNode(att);
    tagInput.addEventListener(
        'keyup',
        function () { fnAutoCompelete(this.value, take, layerName); },
        false
    );

    //برای ذخیره آبجکت آیدی لایه
    var inputObjectID = document.createElement("input");
    inputObjectID.setAttribute("type", "hidden");
    inputObjectID.setAttribute("id", "objectID" + parentElement);
    parEl.appendChild(inputObjectID);

    $("#tagInput").on('input', function () {
        var val = this.value;
        if ($('#tagDatalist').find('option').filter(function () {
            return this.value.toUpperCase() === val.toUpperCase();
        }).length) {

            for (var i = 0; i < arrAutoCompelete.length; i++) {
                if (val == arrAutoCompelete[i][1]) {
                    showOnMap(arrAutoCompelete[i][2], 'OBJECTID', arrAutoCompelete[i][0], 0, true, typeShow, function (res) {

                        //ست کردن مقدار آبجکت آیدی لایه در یک تگ هیدن
                        inputObjectID.setAttribute("value", arrAutoCompelete[i][0]);

                        //mapFn();
                        mapFn1(res);
                    });

                    break;
                }
            }
        }
    });
    var tagDatalist = document.createElement('datalist');
    parEl.appendChild(tagDatalist);
    tagDatalist.id = 'tagDatalist';
}

function fnAutoCompelete(search, take, layerName) {
    var APIurl = webServerURL + "/MapAPIGis/GetAddressBuffer";

    $.ajax({
        type: "POST",
        url: APIurl,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'search': search, 'take': take, 'layerName': layerName }),
        success: function (result) {
            var options = '';
            for (var i = 0; i < result.length; i++) {
                options += '<option value="' + result[i].CompleteAddress + '" />';

                arrAutoCompelete[i] = [];
                arrAutoCompelete[i][0] = result[i].ObjectID;
                arrAutoCompelete[i][1] = result[i].CompleteAddress;
                arrAutoCompelete[i][2] = result[i].ServiceAddress;
            }
            document.getElementById('tagDatalist').innerHTML = options;
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function createSearchtBoxUiLi(take, parentElement, layerName, serviceName) {

    var parEl = document.getElementById(parentElement)
    var tagInput = document.createElement('input');
    parEl.appendChild(tagInput);
    tagInput.id = 'tagInput';
    tagInput.name = 'tagInput';
    tagInput.style = 'font-family:IRANSANS, FontAwesome;border-radius: 25px;padding:8px';
    tagInput.placeholder = 'جستجو';
    tagInput.addEventListener(
        'keyup',
        function () { fnAutoCompeleteUiLi(this.value, take, layerName, serviceName); },
        true
    );

    ulSearch = document.createElement('ui');
    ulSearch.id = 'ulSearchId';
    ulSearch.style = 'list-style-type: none;padding: 0;margin: 0;';
    parEl.appendChild(ulSearch);


    $("#ulSearchId").on('click', 'li', function () {
        var val = this.children[0].text;
        if ($('#ulSearchId').find('li').filter(function () {
            return this.children[0].text.toUpperCase() === val.toUpperCase();
        }).length) {

            tagInput.value = val;
            for (var i = 0; i < arrAutoCompelete.length; i++) {
                if (val == arrAutoCompelete[i][1]) {
                    showOnMap(arrAutoCompelete[i][2], 'OBJECTID', arrAutoCompelete[i][0], 0, true, 'all', function (res) {

                        //$("#btn").click()
                        //var x = parseInt((parseInt(res[0].feature.geometry.cache._extent.xmax) + parseInt(res[0].feature.geometry.cache._extent.xmin)) / 2);
                        //var y = parseInt((parseInt(res[0].feature.geometry.cache._extent.ymax) + parseInt(res[0].feature.geometry.cache._extent.ymin)) / 2);
                        ////setScale(10000);
                        ////var c = getCenterPoint();
                        ////setCoordinate(Math.floor(c.x), Math.floor(c.y));
                        ////setCoordinate(556306, 3614164);
                        ////setScale(1000);
                        //
                        //setCoordinate(x, y, function(){
                        //	setScale(2500)
                        //});
                        //setZoom(0.1);
                        //console.log(res);
                    });
                    break;
                }
            }
            ulSearch.innerHTML = '';
        }
    });

}
var ulSearch;
function fnAutoCompeleteUiLi(search1, take, layerName, serviceName) {
    var APIurl = webServerURL + "/MapAPIGis/GetAddressBuffer";
    ulSearch.innerHTML = '';

    $.ajax({
        type: "POST",
        url: APIurl,
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({'Search': search1.replace('ي','ی'), 'Take' : take, 'LayerName' : layerName}),
        data: JSON.stringify({ 'Search': search1, 'Take': take, 'LayerName': layerName }),
        success: function (result) {
            var li, a;
            arrAutoCompelete = [];
            for (var i = 0; i < result.length; i++) {

                li = document.createElement('li');
                li.style.display = "block";
                a = document.createElement('a');

                a.style = 'border: 1px solid #ddd; margin-top: -1px; background-color: #f6f6f6; padding: 12px; text-decoration: none; font-size: 18px; color: black; display: block';
                a.href = "#";
                li.appendChild(a);

                ulSearch.appendChild(li);
                a.innerHTML = a.innerHTML + result[i].CompleteAddress;

                arrAutoCompelete[i] = [];
                arrAutoCompelete[i][0] = result[i].ObjectID;
                arrAutoCompelete[i][1] = result[i].CompleteAddress;
                arrAutoCompelete[i][2] = result[i].ServiceAddress;
            }
        },
        error: function (e) {
            console.log(e);
        }
    });
}



function zoom(val1) {
    var val = val1;
    for (var i = 0; i < arrAutoCompelete.length; i++) {
        if (val == arrAutoCompelete[i][0]) {
            showOnMap(arrAutoCompelete[i][2], 'OBJECTID', arrAutoCompelete[i][0], 0, true, 'all', function (res) {

            });
            break;
        }
    }
}
//************************************************************
function showOnMap(url, field, value, layerID, isClearMap, typeShow, fn) {
    publicUrl = url;
    publicField = field;
    publicValue = value;
    publicLayerID = layerID;
    publicIsClearMap = isClearMap;
    publicTypeShow = typeShow;
    mapFn = fn;
    dojo.ready(showOnMap1);
}

function showOnMap1() {
    findTask = new esri.tasks.FindTask(publicUrl);
    findParams = new esri.tasks.FindParameters();
    findParams.returnGeometry = true;
    findParams.layerIds = [publicLayerID];
    findParams.searchFields = [publicField];
    findParams.searchText = publicValue;
    findTask.execute(findParams, showResults)
        .addCallback(function (res) {
            mapFn(res);
        });
}

function showResults(results) {

    if (publicIsClearMap == true)
        clearMap();

    //create array of attributes
    var items = dojo._base.array.map(results, function (result) {
        var graphic = result.feature;
        map.graphics.add(graphic);
        return result.feature.attributes;
    });

    for (i = 0; i < items.length; i++)
        ZoomInToLayerWithObjectID(items[i]["OBJECTID"], results, i);

    //if (items.length > 0) {
    //    ZoomInToLayerWithObjectID(items[0]["OBJECTID"], results);
    //    return 1;
    //}
    // else
    //    return 0;
    return 1;
}

//Zoom to the layer with objectID 
function ZoomInToLayerWithObjectID(OBJECTID, results, i) {

    var selectedTaxLot = dojo._base.array.filter(map.graphics.graphics, function (graphic) {
        return ((graphic.attributes) && graphic.attributes.OBJECTID === OBJECTID);
    });

    if (selectedTaxLot.length) {

        if (publicTypeShow == null)
            publicTypeShow = 'all';

        if (publicTypeShow.toLowerCase() == 'point' || publicTypeShow.toLowerCase() == 'all') {
            var marker = new esri.symbol.PictureMarkerSymbol();
            marker.setHeight(40);
            marker.setWidth(40);
            marker.setUrl(webServerURL + '/Content/Images/marker.png');
            map.graphics.add(new esri.Graphic(results[i].feature.geometry, marker));
        }

        switch (selectedTaxLot[0].geometry.type) {
            case "point":
                map.centerAndZoom(new esri.geometry.Point(selectedTaxLot[0].geometry.x, selectedTaxLot[0].geometry.y, new esri.SpatialReference({ wkid: wkid, latestWkid: wkid })), 16);
                break;
            case "polyline":
                if (publicTypeShow.toLowerCase() == 'polyline' || publicTypeShow.toLowerCase() == 'all')
                    var symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 3);
                map.setExtent(selectedTaxLot[0].geometry.getExtent(), true);
                break;
            case "polygon":
                if (publicTypeShow.toLowerCase() == 'polygon' || publicTypeShow.toLowerCase() == 'all')
                    var symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_NONE, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASHDOT, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
                map.setExtent(selectedTaxLot[0].geometry.getExtent(), true);
                break;
        }


        var curFeature = results[i];
        var graphic = curFeature.feature;
        graphic.setSymbol(symbol);
        map.graphics.add(graphic);
    }
}


//function showOnMapByCondition(url, condition, queryProperties, isClearMap, fn) {
//    mapFn = fn;
//    publicIsClearMap = isClearMap;

//    if (queryProperties == "") {
//        queryProperties = {
//            spatialRel: "esriSpatialRelIntersects",
//            geometryType: "esriGeometryEnvelope",
//        };
//    }

//    queryTask = new esri.tasks.QueryTask(url);

//    //initialize query
//    query = new esri.tasks.Query();
//    query.where = condition;
//    query.outFields = ["*"];
//    query.geometryType = queryProperties.geometryType;
//    query.geometry = queryProperties.geometry;
//    query.spatialRel = queryProperties.spatialRel;
//    query.returnGeometry = true;
//    query.outSpatialReference = { "wkid": wkid };
//    ////query.returnIdsOnly = false;
//    ////query.returnCountOnly = false;
//    ////query.returnDistinctValues = false;

//    queryTask.execute(query, showResultsByCondition);
//}

//function showResultsByCondition(featureSet) {
//    ////remove all graphics on the maps graphics layer
//    if (publicIsClearMap == true)
//        clearMap();

//    var marker = new esri.symbol.PictureMarkerSymbol();
//    marker.setHeight(30);
//    marker.setWidth(30);
//    marker.setUrl(webServerURL + '/Content/Images/marker.png');
//    var symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_NONE, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASHDOT, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
//    var lineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 3);
//    var pointSymbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_SQUARE, 10, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 1), new dojo.Color([0, 255, 0, 0.25]));

//    //Performance enhancer - assign featureSet array to a single variable.
//    var resultFeatures = featureSet.features;

//    //Loop through each feature returned
//    for (var i = 0, il = resultFeatures.length; i < il; i++) {
//        //Get the current feature from the featureSet.
//        //Feature is a graphic
//        var graphic = resultFeatures[i];

//        ////Add graphic to the map graphics layer.
//        switch (graphic.geometry.type) {
//            case 'polygon':
//                var polygon = new esri.geometry.Polygon(graphic.geometry.rings);
//                map.graphics.add(new esri.Graphic(polygon, symbol));
//                break;

//            case 'point':
//                var point = new esri.geometry.Point(graphic.geometry.x, graphic.geometry.y);
//                map.graphics.add(new esri.Graphic(point, pointSymbol));
//                break;

//            case 'polyline':
//                var line = new esri.geometry.Polyline(graphic.geometry.paths)
//                map.graphics.add(new esri.Graphic(line, lineSymbol));
//                break;

//            default:
//                break;
//        }

//        //map.graphics.add(graphic);

//    }

//    mapFn(resultFeatures);
//}

//---------------------------------------------------------------


function showOnMapByGraphicsLayer(url, condition, queryProperties, isClearMap, userLayerId, fn) {
    mapFn = fn;
    publicIsClearMap = isClearMap;
    publicUserLayerId = userLayerId;

    if (queryProperties == "") {
        queryProperties = {
            spatialRel: "esriSpatialRelIntersects",
            geometryType: "esriGeometryEnvelope",
            //geometry: map.extent,
        };
    }

    queryTask = new esri.tasks.QueryTask(url);

    //initialize query
    query = new esri.tasks.Query();
    query.where = condition;
    query.outFields = ["*"];
    query.geometryType = queryProperties.geometryType;
    query.geometry = queryProperties.geometry;
    query.spatialRel = queryProperties.spatialRel;
    query.returnGeometry = true;
    query.outSpatialReference = { "wkid": wkid };
    ////query.returnIdsOnly = false;
    ////query.returnCountOnly = false;
    ////query.returnDistinctValues = false;

    queryTask.execute(query, showResultsByGraphicsLayer);
}

function showResultsByGraphicsLayer(featureSet) {
    ////remove all graphics on the maps graphics layer
    if (publicIsClearMap == true)
        clearMap();


    var symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_NONE, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASHDOT, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
    var lineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 3);
    var pointSymbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_SQUARE, 20, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 1), new dojo.Color([0, 255, 0, 0.25]));

    //Performance enhancer - assign featureSet array to a single variable.
    var resultFeatures = featureSet.features;

    ////Create graphics layer for counties
    var userGraphicsLayer = new esri.layers.GraphicsLayer({ id: "userLayer" + publicUserLayerId });
    if (!map.graphicsLayerIds.includes("userLayer" + publicUserLayerId))
        map.addLayer(userGraphicsLayer);


    //Loop through each feature returned
    for (var i = 0, il = resultFeatures.length; i < il; i++) {
        //Get the current feature from the featureSet.
        //Feature is a graphic
        var graphic = resultFeatures[i];

        ////Add graphic to the map graphics layer.
        switch (graphic.geometry.type) {
            case 'polygon':
                userGraphicsLayer.add(graphic.setSymbol(symbol));
                map.setExtent(graphic.geometry.getExtent(), true);
                break;

            case 'point':
                userGraphicsLayer.add(graphic.setSymbol(pointSymbol));
                map.centerAndZoom(new esri.geometry.Point(graphic.geometry.x, graphic.geometry.y, new esri.SpatialReference({ wkid: wkid, latestWkid: wkid })), 16);
                break;

            case 'polyline':

                userGraphicsLayer.add(graphic.setSymbol(lineSymbol));
                map.setExtent(graphic.geometry.getExtent(), true);
                break;

            default:
                break;
        }

        //map.graphics.add(graphic);

    }

    mapFn(resultFeatures);
}



///
/// typeShow = polygon - polyline - point - marker ** polygon1 - polyline1 - point1 - marker1
///
function showOnMapByGraphicsLayerForSearch(url, condition, queryProperties, isClearMap, userLayerId, typeShow, fn) {
    mapFn = fn;
    publicIsClearMap = isClearMap;
    publicUserLayerId = userLayerId;
    publicTypeShow = typeShow;

    if (queryProperties == "") {
        queryProperties = {
            spatialRel: "esriSpatialRelIntersects",
            geometryType: "esriGeometryEnvelope",
            //geometry: map.extent,
        };
    }

    queryTask = new esri.tasks.QueryTask(url);

    //initialize query
    query = new esri.tasks.Query();
    query.where = condition;
    query.outFields = ["*"];
    query.geometryType = queryProperties.geometryType;
    query.geometry = queryProperties.geometry;
    query.spatialRel = queryProperties.spatialRel;
    query.returnGeometry = true;
    query.outSpatialReference = { "wkid": wkid };
    ////query.returnIdsOnly = false;
    ////query.returnCountOnly = false;
    ////query.returnDistinctValues = false;

    queryTask.execute(query, showResultsByGraphicsLayerForSearch);
}

function showResultsByGraphicsLayerForSearch(featureSet) {
    ////remove all graphics on the maps graphics layer
    if (publicIsClearMap == true) {
        clearMap();
        clearMapGraphic("searchLayer");
    }


    var marker = new esri.symbol.PictureMarkerSymbol();
    marker.setHeight(30);
    marker.setWidth(30);
    marker.setUrl(webServerURL + '/Content/Images/marker.png');
    var symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_NONE, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASHDOT, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
    var lineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 3);
    var pointSymbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_SQUARE, 20, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 1), new dojo.Color([0, 255, 0, 0.25]));

    //Performance enhancer - assign featureSet array to a single variable.
    var resultFeatures = featureSet.features;

    ////Create graphics layer for counties
    var userGraphicsLayer = new esri.layers.GraphicsLayer({ id: "searchLayer" + publicUserLayerId });
    if (!map.graphicsLayerIds.includes("searchLayer" + publicUserLayerId))
        map.addLayer(userGraphicsLayer);


    //Loop through each feature returned
    for (var i = 0, il = resultFeatures.length; i < il; i++) {
        //Get the current feature from the featureSet.
        //Feature is a graphic
        var graphic = resultFeatures[i];

        ////Add graphic to the map graphics layer.
        switch (graphic.geometry.type) {
            case 'polygon':
                userGraphicsLayer.add(graphic.setSymbol(symbol));
                map.setExtent(graphic.geometry.getExtent(), true);
                break;

            case 'point':

                if (publicTypeShow == "marker")
                    userGraphicsLayer.add(graphic.setSymbol(marker));
                else
                    userGraphicsLayer.add(graphic.setSymbol(pointSymbol));

                map.centerAndZoom(new esri.geometry.Point(graphic.geometry.x, graphic.geometry.y, new esri.SpatialReference({ wkid: wkid, latestWkid: wkid })), 16);
                break;

            case 'polyline':

                userGraphicsLayer.add(graphic.setSymbol(lineSymbol));
                map.setExtent(graphic.geometry.getExtent(), true);
                break;

            default:
                break;
        }

        //map.graphics.add(graphic);

    }

    mapFn(resultFeatures);
}




//Zoom to the layer with latitude & longitude
function ZoomInToLayerWithLatitudeLongitude(latitude, longitude) {
    globalLatitude = latitude;
    globalLongitude = longitude;
    dojo.ready(ZoomInToLayerWithLatitudeLongitude1);
}

function ZoomInToLayerWithLatitudeLongitude1() {
    setScale(5000);
    map.centerAndZoom(new esri.geometry.Point(globalLatitude, globalLongitude, new esri.SpatialReference({ wkid: wkid })), 0.3);
}

////////set Extent/////////
function setExtent(url, field, value, layerID, fn) {
    publicUrl = url;
    publicField = field;
    publicValue = value;
    publicLayerID = layerID;
    mapFn = fn;
    dojo.ready(setExtent1);
}

function setExtent1() {
    findTask = new esri.tasks.FindTask(publicUrl);
    findParams = new esri.tasks.FindParameters();
    findParams.returnGeometry = true;
    findParams.layerIds = [publicLayerID];
    findParams.searchFields = [publicField];
    findParams.searchText = publicValue;
    findTask.execute(findParams, showResultsExtent)
        .addCallback(function (res) {
            mapFn(res);
        });
}

function showResultsExtent(results) {
    //create array of attributes
    var items = dojo._base.array.map(results, function (result) {
        var graphic = result.feature;
        map.graphics.add(graphic);
        return result.feature.attributes;
    });

    if (items.length > 0) {
        setExtentWithObjectID(items[0]["OBJECTID"]);
        return 1;
    }
    else
        return 0;
}

//Zoom to the layer with objectID 
function setExtentWithObjectID(OBJECTID) {
    var selectedTaxLot = dojo._base.array.filter(map.graphics.graphics, function (graphic) {
        return ((graphic.attributes) && graphic.attributes.OBJECTID === OBJECTID);
    });
    //if (selectedTaxLot.length) {
    //	map.setExtent(selectedTaxLot[0].geometry.getExtent(), true);
    //}

    for (var i = 0; i < selectedTaxLot.length; i++) {
        if (selectedTaxLot[i].attributes[publicField] == publicValue) {
            map.setExtent(selectedTaxLot[i].geometry.getExtent(), true);
            break;
        }
    }
}

function clearMap() {
    map.graphics.clear();
}

function clearMapGraphic(filter) {
    var graphicsLayerIdsFilter = map.graphicsLayerIds.filter(function (item) { return item.includes(filter) })
    for (var i = 0; i < graphicsLayerIdsFilter.length; i++) {
        map.removeLayer(map.getLayer(graphicsLayerIdsFilter[i]));
    }
}

function removeLayers() {
    if (map == null)
        return;
    map.removeAllLayers();
}

/** @description Return featur and attributes on point click.  
 * @param {event} event click.  
 * @param {fn} callback method.  
 * @return {object}  
 */
function getInfoClick(event, fn) {
    if (event.mapPoint == null)
        return;

    //if (event == null || event.graphic == null || event.graphic.geometry == null)
    //    return;

    //if (event.graphic.geometry.type == "polygon") {
    //    URLSearch = userLayerPolygonURL
    //}
    //else if (event.graphic.geometry.type == "polyline") {
    //    URLSearch = userLayerLineURL;
    //}
    //else if (event.graphic.geometry.type == "point") {
    //    URLSearch = userLayerPointURL;
    //}

    identifyTask = new esri.tasks.IdentifyTask(URLSearch);
    identifyParams.geometry = event.mapPoint;
    identifyParams.mapExtent = map.extent;
    //identifyParams.layerIds = 0;

    var deferred = identifyTask
        .execute(identifyParams)
        .addCallback(function (response) {
            // response is an array of identify result objects
            // Let's return an array of features.
            return dojo._base.array.map(response, function (result) {
                fn(result);
            });

        });
}



//function onErrorReceived(error) {
//    $(divName).remove();
//}

function treeViewMap(parentName, groupService, layersAccess) {
    $.ajax({
        type: "POST",
        url: webServerURL + '/WebGIS/MapAPIGis/MapLayer',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'groupService': groupService, 'layersAccess': layersAccess }),
        success: function (results) {
            var tree = _makeTree({ q: results }, parentName);
            var a = 0;
        },
        error: function (e) {
            console.log(e);
        }
    });
}


function mapClick(event) {
    callMapClick(event);
}


function updateEnd(event) {
    //$('#map_graphics_layer image').css('position', 'fixed');
    //$('#map_graphics_layer image').attr('x', 100);
    //$('#map_graphics_layer image').attr('y', 100);
    //$('#map_graphics_layer image').attr('test', 'fdsafsad');
}

function mapExtentChange(event) {

    if (map == null)
        return 0;

    //$("#map_gc").css({"transform":"translate(0px, 0px)" });
    //$("#map_gc").css({ "background-position": "center", "background-repeat": "no-repeat", "background-size": "cover", "transform":"translate(0px, 0px)" });
}

function setScale(scale) {
    if (map == null)
        return 5000;
    map.setScale(scale);
}

function getScale() {
    if (map == null)
        return 5000;
    return map.getScale();
}

function setCoordinate(x, y, fn) {
    if (map != null) {
        map.centerAndZoom(new esri.geometry.Point(x, y, new esri.SpatialReference({ wkid: wkid, latestWkid: wkid })), 16);
    }
    fn();
}

function setCoordinateWithMarker(x, y, fn) {
    if (map != null) {
        map.centerAndZoom(new esri.geometry.Point(x, y, new esri.SpatialReference({ wkid: wkid, latestWkid: wkid })), 18);
          //map.centerAndZoom(new esri.geometry.Point(x, y, new esri.SpatialReference({ wkid: wkid })), 1);

        clearMap();
        var point = new esri.geometry.Point(x, y); //getCenterPoint();
        var marker = new esri.symbol.PictureMarkerSymbol();
        marker.setHeight(30);
        marker.setWidth(30);
        marker.setUrl(webServerURL + '/Content/Images/marker.png');
        var graphic = new esri.Graphic(point, marker);
        map.graphics.add(graphic);
    }

    fn();
}

function getCenterPoint() {
    if (map == null)
        return 0;
    return map.extent.getCenter();
}


// If you are using a basemap with spatial reference is 102100 (web mercator) and you would just use webMercatorUtils.geographictowebmercator
// to convert to x, y
function convertCordinate(x, y) {
    var result = null;
    var point = new esri.geometry.Point(x, y, map.spatialReference);
    if (esri.geometry.canProject(point, map)) {
        result = esri.geometry.project(point, map);
    }

    return result;
}


function measurementTools(measurementDiv) {

    var divMeasure = document.getElementById(measurementDiv);

    var divCalcite = document.createElement('div');
    divCalcite.class = "calcite";
    divMeasure.appendChild(divCalcite);

    var divMeasurement1 = document.createElement('div');
    divMeasurement1.id = "divMeasurement1";
    divCalcite.appendChild(divMeasurement1);

    var divTitlePane = document.createElement('div');
    divTitlePane.id = "titlePane";
    //divTitlePane.datadojotype = "dijit/TitlePane";
    //divTitlePane.datadojoprops = "title:'Measurement', closable:fals";
    divMeasurement1.appendChild(divTitlePane);

    if (measure == null) {
        measure = new esri.dijit.Measurement({
            map: map
        }, dojo.byId(measurementDiv));
        measure.startup();
    }

    var divMeasure1 = document.getElementById(measurementDiv);
    divMeasure1.style = "position: relative; background-color: lightgray; width: 300px; height: 150px; border-radius: 10px; direction: rtl; border-style: solid; margin: 15px; padding: 5px; position: absolute;  z-index: 99; right: 0px;";
}


function addLayerToMap(url) {
    layer = new esri.layers.ArcGISDynamicMapServiceLayer(url);
    map.addLayer(layer);
}

function removeLayerFromMap(url) {
    layer = new esri.layers.ArcGISDynamicMapServiceLayer(url);
    map.removeLayer(layer);
}

function printTools(mapDiv, pageTite) {


    $('#' + mapDiv).printThis({
        pageTitle: pageTite
    });


}

function printToolsEsri(title, _dpi, layot, fn) {

    mapFn = fn;


    var divTempPrint = document.createElement('div');
    divTop1Map.appendChild(divTempPrint);
    divTempPrint.id = 'divTempPrint';

    var urlTempPrint = "https://mahmoodkabi.ir/arcgis/rest/services/UserLayer/UserLayerPoint/MapServer"
    var mapTempPrint = new esri.Map(divTempPrint);
    var layerTempPrint = new esri.layers.ArcGISDynamicMapServiceLayer(urlTempPrint);
    mapTempPrint.addLayer(layerTempPrint);

    var printUrl = "https://mahmoodkabi.ir/arcgis/rest/services/Utilities/PrintingTools/GPServer/Export%20Web%20Map%20Task";

    //Set up print stuff
    var printTask = new esri.tasks.PrintTask(printUrl);
    var params = new esri.tasks.PrintParameters();
    var template = new esri.tasks.PrintTemplate();

    params.map = map;
    template.exportOptions = {
        width: 3508,
        height: 2480,
        dpi: _dpi
    };
    template.layout = layot;
    template.preserveScale = true;

    template.layoutOptions = {
        titleText: title,
        authorText: "ردفان",
        copyrightText: "کلیه حقوق محفوظ می باشد",
        copyrightText: "",
        scalebarUnit: "CM",
    }


    params.template = template;

    dojo.connect(mapTempPrint, "onLoad", function () {//Fire the print task
        printTask.execute(params, printResult);
    });
    //printTask.execute(params, printResult);

    var printResult = function (result) {
        //$("divTempPrint").remove();
        mapFn(result);
    }
}


function addGraphicToMap(fn) {
    mapFn = fn;

    // fill symbol used for extent, polygon and freehand polygon, use a picture fill symbol
    // the images folder contains additional fill images, other options: sand.png, swamp.png or stiple.png
    fillSymbol = new esri.symbol.PictureFillSymbol(
        webServerURL + "/Content/Images/RedfunLogo.png",
        new esri.symbol.SimpleLineSymbol(
            esri.symbol.SimpleLineSymbol.STYLE_SOLID,
            new dojo.Color('#000'),
            1
        ),
        42,
        42
    );

    tb = new esri.toolbars.Draw(map);
    tb.on("draw-end", addGraphic);

    // Create and setup editing tools
    editToolbar = new esri.toolbars.Edit(map);

    map.on("click", function (evt) {
        editToolbar.deactivate();
    });

    createMapMenu();
    createGraphicsMenu();

    // event delegation so a click handler is not
    // needed for each individual button
    dojo.on(dojo.dom.byId("draw-info"), "click", function (evt) {
        if (evt.target.id === "draw-info") {
            return;
        }
        var tool = evt.target.id.toLowerCase();
        map.disableMapNavigation();
        tb.activate(tool);
    });
}

function addGraphic(evt) {
    //deactivate the toolbar and clear existing graphics 
    tb.deactivate();
    map.enableMapNavigation();

    // figure out which symbol to use
    var symbol;
    if (evt.geometry.type === "point" || evt.geometry.type === "multipoint") {
        symbol = markerSymbol;
    } else if (evt.geometry.type === "line" || evt.geometry.type === "polyline") {
        symbol = lineSymbol;
    }
    else {
        symbol = fillSymbol;
    }

    map.graphics.add(new esri.Graphic(evt.geometry, symbol));

    //کال بک
    //هر عملیاتی که می خواهیم بعد از رسم پلیگان  بر روی نقشه انجام شود
    mapFn();
}



function createMapMenu() {
    // Creates right-click context menu for map
    ctxMenuForMap = new dijit.Menu({
        onOpen: function (box) {
            // Lets calculate the map coordinates where user right clicked.
            // We'll use this to create the graphic when the user clicks
            // on the menu item to "Add Point"
            currentLocation = getMapPointFromMenuPosition(box);
            editToolbar.deactivate();
        }
    });

    //ctxMenuForMap.addChild(new dijit.MenuItem({ 
    //label: "Add Point",
    //onClick: function(evt) {
    //  var symbol = new esri.symbol.SimpleMarkerSymbol(
    //	esri.symbol.SimpleMarkerSymbol.STYLE_SQUARE, 
    //	30, 
    //	new esri.symbols.SimpleLineSymbol(
    //	  esri.symbols.SimpleLineSymbol.STYLE_SOLID, 
    //	  new dojo.Color([200,235, 254, 0.9]), 
    //	  2
    //	), new dojo.Color([200, 235, 254, 0.5]));
    //  var graphic = new esri.graphic(geometryJsonUtils.fromJson(currentLocation.toJson()), symbol);
    //  map.graphics.add(graphic);
    //}
    //}));

    ctxMenuForMap.startup();
    ctxMenuForMap.bindDomNode(map.container);
}

function createGraphicsMenu() {
    // Creates right-click context menu for GRAPHICS
    ctxMenuForGraphics = new dijit.Menu({});

    ctxMenuForGraphics.addChild(new dijit.MenuItem({
        label: "ویرایش",
        onClick: function () {

            // //if(selected.geometry.type == "extent"){
            //	//  var polygon = new esri.geometry.Polygon(new esri.SpatialReference({wkid:wkid}));
            //	//  polygon.addRing([[selected.geometry.xmin,selected.geometry.ymin],
            //	//				   [selected.geometry.xmin,selected.geometry.ymax],
            //	//				   [selected.geometry.xmax,selected.geometry.ymax],
            //	//				   [selected.geometry.xmax,selected.geometry.ymin],
            //	//				   [selected.geometry.xmin,selected.geometry.ymin]]);
            // //    
            //	//  selected = polygon;
            //	//   editToolbar.activate(esri.toolbars.Edit.EDIT_VERTICES, selected);  
            // //}


            if (selected.geometry.type !== "point") {
                editToolbar.activate(esri.toolbars.Edit.EDIT_VERTICES, selected);
            } else {
                editToolbar.activate(esri.toolbars.Edit.MOVE | esri.toolbars.Edit.EDIT_VERTICES | esri.toolbars.Edit.EDIT_TEXT, selected);
            }
        }
    }));

    ctxMenuForGraphics.addChild(new dijit.MenuItem({
        label: "جابه جایی",
        onClick: function () {
            editToolbar.activate(esri.toolbars.Edit.MOVE, selected);
        }
    }));

    //ctxMenuForGraphics.addChild(new dijit.MenuItem({ 
    //label: "چرخش / مقیاس",
    //onClick: function() {
    //if ( selected.geometry.type !== "point" ) {
    //	editToolbar.activate(esri.toolbars.Edit.ROTATE | Edit.SCALE, selected);
    //  } else {
    //	alert("Not implemented");
    //  }
    //}
    //}));

    //ctxMenuForGraphics.addChild(new dijit.MenuItem({ 
    //label: "Style",
    //onClick: function() {
    //  alert("Not implemented");
    //}
    //}));

    ctxMenuForGraphics.addChild(new dijit.MenuSeparator());
    ctxMenuForGraphics.addChild(new dijit.MenuItem({
        label: "حذف",
        onClick: function () {
            map.graphics.remove(selected);
        }
    }));

    ctxMenuForGraphics.startup();

    map.graphics.on("mouse-over", function (evt) {
        // We'll use this "selected" graphic to enable editing tools
        // on this graphic when the user click on one of the tools
        // listed in the menu.
        selected = evt.graphic;

        // Let's bind to the graphic underneath the mouse cursor           
        ctxMenuForGraphics.bindDomNode(evt.graphic.getDojoShape().getNode());
    });

    map.graphics.on("mouse-out", function (evt) {
        ctxMenuForGraphics.unBindDomNode(evt.graphic.getDojoShape().getNode());
    });
}

// Helper Methods
function getMapPointFromMenuPosition(box) {
    var x = box.x, y = box.y;
    switch (box.corner) {
        case "TR":
            x += box.w;
            break;
        case "BL":
            y += box.h;
            break;
        case "BR":
            x += box.w;
            y += box.h;
            break;
    }

    var screenPoint = new esri.geometry.Point(x - map.position.x, y - map.position.y);
    return map.toMap(screenPoint);
}


var bookmarks;
var itemsBookmark = [];
function mapBookmarks() {
    bookmarks.bookmarks.forEach(function (item) {
        var objBookmark = {
            Xmax: item.extent.xmax,
            Ymax: item.extent.ymax,
            Xmin: item.extent.xmin,
            Ymin: item.extent.ymin,
            Wkid: item.extent.spatialReference.wkid,
            Name: item.name,
            Description: ''
        }
        itemsBookmark.push(objBookmark);
    });
    return itemsBookmark;
}

function setExtentByExtent(extentMap) {
    var extent = new esri.geometry.Extent({
        "xmin": extentMap.Xmin, "ymin": extentMap.Ymin, "xmax": extentMap.Xmax, "ymax": extentMap.Ymax,
        "spatialReference": { "wkid": wkid, "latestWkid": wkid }
    });

    //var extent = new esri.geometry.Extent(extentMap.Xmin, extentMap.Ymin, extentMap.Xmax, extentMap.Ymax, new esri.SpatialReference({ wkid: wkid }));
    map.setExtent(extent, true);
}




function navigationPreNextExtent(type) {


    if (type == "Next") {
        // dijit.registry.byId("zoomnext").on("click", function () {
        navToolbar.zoomToNextExtent();
        // });
    }
    else { // priviouse
        //  dijit.registry.byId("zoomprev").on("click", function () {
        navToolbar.zoomToPrevExtent();
        //   });
    }
}


function extentHistoryChangeHandler() {
    dijit.registry.byId("zoomprev").disabled = navToolbar.isFirstExtent();
    dijit.registry.byId("zoomnext").disabled = navToolbar.isLastExtent();
}



//ابزار رسم شکل بر روي نقشه
function createDivGraphgic(parentId, typeofBox) {
    var res = "";



    var boxGraphic = "<div class='options'><hr><div class='options-inner'>" +
        "<form class='td-controls' data-action='tools-options-update' data-method=''>" +
        "<div class='td-control td-control-icon'>" +
        "       <label>ترسیم</label>" +
        "       <div id = 'draw-info' style='margin-top:10px;'>" +
        "           <i id='Circle' class='icon icon-tool-circle' data-toggle='tooltip' data-value='circle' title='رسم دایره'></i>" +
        "           <i id='Extent' class='icon icon-tool-rectangle' data-toggle='tooltip' data-value='rectangle' title='رسم مستطیل'></i>" +
        "           <i id='Polygon' class='icon icon-tool-polygon' data-toggle='tooltip' data-value='polygon' title='رسم چند ضلعی'></i>" +
        "       </div>" +
        "   </div>" +
        " <div id = 'subTollDraw'></div>" +
        "</form>";
    "</div>" +
        "</div>";

    document.getElementById(parentId).innerHTML = boxGraphic;



    //createBoxPoint("subTollDraw");
    ////لود كردن ابزار متناسب با نوع ترسيم
    $(".td-control.td-control-icon i").on("click", function (e) {
        //typeofBox = $(this).attr("data-value");
        ////create box for point
        //switch (typeofBox) {
        //    case "point":
        //        res = createBoxPoint("subTollDraw");
        //        break;
        //    case "rectangle":
        //        res = createBoxRectangle("subTollDraw");
        //        break;
        //    case "line":
        //        res = createBoxLine();
        //        break;
        //    case "polygon":
        //        // res = createBoxPolygon();
        //        break;
        //    case "cricle":
        //        res = createBoxCricle();
        //        break;
        //    case "text":
        //        //res = createBoxText();
        //        break;
        //    default:
        //        break;
        //}


        //createDivGraphgic("drawShape", $(this).attr("data-value"));
        $("#main-tools .options").css("display", "block");

        //فعال و غير فعال كردن ابزار رسم شكل هاي وخط و نقطه
        $(".td-control.td-control-icon i").removeClass("icon-subtoll active");
        $(this).addClass("icon-subtoll active");
    })

    //فعال و غير فعال كردن رنگ انتخاب شده براي Fill
    $('body').on('click', '.td-control.td-control-color.style-color ul.color-selector li', function (event) {
        $(".td-control.td-control-color.style-color ul.color-selector li").removeClass("selected");
        $(this).addClass("selected");
    });

    //فعال و غير فعال كردن رنگ انتخاب شده براي Fill
    $('body').on('click', '.td-control.td-control-color.stroke-color ul.color-selector li', function (event) {
        $(".td-control.td-control-color.stroke-color ul.color-selector li").removeClass("selected");
        $(this).addClass("selected");
    });
}

function createBoxPoint(parentIDSubtoll) {

    var boxGraphicPoint = "<div class='td-control td-control-select'>" +
        "<label>Style</label>" +
        "   <select name='symbol-style'>" +
        "       <option value='circle' selected=''>Circle</option>" +
        "       <option value='cross'>Cross</option>" +
        "       <option value='diamond'>Diamond</option>" +
        "       <option value='square'>Square</option>" +
        "   </select>" +
        "</div>" +

        "<div class='td-control td-control-color style-color'>" +
        "    <label>Fill</label>" +
        "   <ul class='color-selector'>" +
        "       <input type='hidden' name='symbol-color' value='#ffffff'>" +
        "       <li data-value='#8dc63f' class='color  ' style='background-color:#8dc63f;'></li>" +
        "       <li data-value='#00aeef' class='color  ' style='background-color:#00aeef;'></li>" +
        "       <li data-value='#b47fe2' class='color  ' style='background-color:#b47fe2;'></li>" +
        "       <li data-value='#ec008c' class='color  ' style='background-color:#ec008c;'></li>" +
        "       <li data-value='#f7941d' class='color  ' style='background-color:#f7941d;'></li>" +
        "       <li data-value='#ffff00' class='color  ' style='background-color:#ffff00;'></li>" +
        "       <li data-value='#ffffff' class='color  ' style='background-color:#ffffff;'></li>" +
        "       <li data-value='#000000' class='color  ' style='background-color:#000000;'></li>" +
        "   </ul>" +
        "</div>" +

        "<div class='td-control td-control-range'>" +
        "   <div class='slider-value'>50%</div>" +
        "   <label>Fill Opacity</label>" +
        "   <input type='hidden' name='symbol-opacity' value='50' autocomplete='off'>" +
        "   <div class='slider-attribute ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all' data-value='1,100,50,%' aria-disabled='false'><div class='ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min' style='width: 49.4949%;'></div><a class='ui-slider-handle ui-state-default ui-corner-all' href='#' style='left: 49.4949%;'></a></div>" +
        "</div>" +

        "<div class='td-control td-control-range'>" +
        "   <div class='slider-value'>12px</div>" +
        "   <label>Size</label>" +
        "   <input type='hidden' name='symbol-size' value='12' autocomplete='off'>" +
        "   <div class='slider-attribute ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all' data-value='4,52,12,px' aria-disabled='false'><div class='ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min' style='width: 16.6667%;'></div><a class='ui-slider-handle ui-state-default ui-corner-all' href='#' style='left: 16.6667%;'></a></div>" +
        "</div>" +

        "<div class='td-control td-control-color stroke-color'>" +
        "   <label>Stroke</label>" +
        "   <ul class='color-selector'>" +
        "       <input type='hidden' name='symbol-outline-color' value='#ffffff'>" +
        "       <li data-value='#8dc63f' class='color  ' style='background-color:#8dc63f;'></li>" +
        "       <li data-value='#00aeef' class='color  ' style='background-color:#00aeef;'></li>" +
        "       <li data-value='#b47fe2' class='color  ' style='background-color:#b47fe2;'></li>" +
        "       <li data-value='#ec008c' class='color  ' style='background-color:#ec008c;'></li>" +
        "       <li data-value='#f7941d' class='color  ' style='background-color:#f7941d;'></li>" +
        "       <li data-value='#ffff00' class='color  ' style='background-color:#ffff00;'></li>" +
        "       <li data-value='#ffffff' class='color  ' style='background-color:#ffffff;'></li>" +
        "       <li data-value='#000000' class='color  ' style='background-color:#000000;'></li>" +
        "   </ul>" +
        "</div>" +

        "<div class='td-control td-control-range td-last'>" +
        "   <div class='slider-value'>3px</div>" +
        "   <label>Stroke Width</label>" +
        "   <input type='hidden' name='symbol-outline-width' value='3' autocomplete='off'>" +
        "   <div class='slider-attribute ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all' data-value='2,12,3,px' aria-disabled='false'><div class='ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min' style='width: 10%;'></div><a class='ui-slider-handle ui-state-default ui-corner-all' href='#' style='left: 10%;'></a></div>" +
        "</div>";



    document.getElementById(parentIDSubtoll).innerHTML = boxGraphicPoint;
}

function createBoxRectangle(parentIDSubtoll) {

    var boxGraphicRectangle = "<div class='td-control td-control-color style-color'>" +
        "<label>Fill</label>" +
        "<ul class='color-selector'>" +
        "<input type='hidden' name='symbol-color' value='#ffffff'>" +
        "<li data-value='#8dc63f' class='color  ' style='background-color:#8dc63f;'></li>" +
        "<li data-value='#00aeef' class='color  ' style='background-color:#00aeef;'></li>" +
        "<li data-value='#b47fe2' class='color  ' style='background-color:#b47fe2;'></li>" +
        "<li data-value='#ec008c' class='color  ' style='background-color:#ec008c;'></li>" +
        "<li data-value='#f7941d' class='color  ' style='background-color:#f7941d;'></li>" +
        "<li data-value='#ffff00' class='color  ' style='background-color:#ffff00;'></li>" +
        "<li data-value='#ffffff' class='color  ' style='background-color:#ffffff;'></li>" +
        "<li data-value='#000000' class='color  ' style='background-color:#000000;'></li>" +
        "</ul>" +
        "</div > " +
        "<div class='td-control td-control-range'>" +
        "<div class='slider-value'>15%</div>" +
        " <label>Fill Opacity</label>" +
        " <input type='hidden' name='symbol-opacity' value='15' autocomplete='off'>" +
        " <div class='slider-attribute ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all' data-value='1,100,15,%' aria-disabled='false'><div class='ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min' style='width: 14.1414%;'></div><a class='ui-slider-handle ui-state-default ui-corner-all' href='#' style='left: 14.1414%;'></a></div>" +
        "</div>" +
        "<div class='td-control td-control-color stroke-color'>" +
        "<label>Stroke</label>" +
        "<ul class='color-selector'>" +
        " <li data-value='#8dc63f' class='color  ' style='background-color:#8dc63f;'></li>" +
        " <li data-value='#00aeef' class='color  ' style='background-color:#00aeef;'></li>" +
        " <li data-value='#b47fe2' class='color  ' style='background-color:#b47fe2;'></li>" +
        " <li data-value='#ec008c' class='color  ' style='background-color:#ec008c;'></li>" +
        "<li data-value='#f7941d' class='color  ' style='background-color:#f7941d;'></li>" +
        "<li data-value='#ffff00' class='color  ' style='background-color:#ffff00;'></li>" +
        " <li data-value='#ffffff' class='color ' style='background-color:#ffffff;'></li>" +
        " <li data-value='#000000' class='color  ' style='background-color:#000000;'></li>" +
        "</ul>" +
        "</div>" +
        "<div class='td-control td-control-range td-last'>" +
        " <div class='slider-value'>3px</div>" +
        "  <label>Stroke Width</label>" +
        "  <input type='hidden' name='symbol-outline-width' value='3' autocomplete='off'>" +
        "<div class='slider-attribute ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all' data-value='2,12,3,px' aria-disabled='false'><div class='ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min' style='width: 10%;'></div><a class='ui-slider-handle ui-state-default ui-corner-all' href='#' style='left: 10%;'></a></div>" +
        " </div>" +
        " </form>" +
        " </div>" +
        " </div>";

    document.getElementById(parentIDSubtoll).innerHTML = boxGraphicRectangle;
}


function addRings(rings) {
    clearMap();
    //var polygonJson = JSON.parse(rings);
    var polygon = new esri.geometry.Polygon(rings);
    var symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_NONE, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASHDOT, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
    map.graphics.add(new esri.Graphic(polygon, symbol));
}

function addKMLToMap(kmlUrl) {
    var kml = new esri.layers.KMLLayer(kmlUrl);
    map.addLayer(kml);
    kml.on("load", function () {
        //domStyle.set("loading", "display", "none");
    });
}