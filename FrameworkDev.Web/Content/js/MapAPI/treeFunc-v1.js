var arrService = [];
$(function () {

    $('#treeviewService input[type="checkbox"]').change(checkboxChanged);

    function checkboxChanged() {
        var $this = $(this),
            checked = $this.prop("checked"),
            container = $this.parent(),
            siblings = container.siblings();

        container.find('input[type="checkbox"]')
        .prop({
            indeterminate: false,
            checked: checked
        })
        .siblings('label')
        .removeClass('custom-checked custom-unchecked custom-indeterminate')
        .addClass(checked ? 'custom-checked' : 'custom-unchecked');

        $('#treeviewService input[type=checkbox]:checked').each(function () {
            arrService.indexOf($(this).val()) === -1 ? arrService.push($(this).val()) : console.log("This item already exists");
        });

        $('#treeviewService input[type=checkbox]:not(:checked)').each(function () {
            var index = arrService.indexOf($(this).val());
            if (index > -1) {
                arrService.splice(index, 1);
            }
        });


        callServices();
    }

	function getMapLayers() {
		//for (var j=0, jl=map.layerIds.length; j<jl; j++) {
		//	var currentLayer = map.getLayer(map.layerIds[j]);
		//	
		//	if(j != 0)
		//	  map.removeLayer(currentLayer);
		//	//alert("id: " + currentLayer.id);
		//}
		
		while(map.layerIds.length > 1){
			var currentLayer = map.getLayer(map.layerIds[map.layerIds.length - 1])
			map.removeLayer(currentLayer);
		}
	}

    function callServices() {
        var URL, layer;
        //map.removeAllLayers();
		getMapLayers();
		
		
		//layer = new esri.layers.ArcGISDynamicMapServiceLayer("http://80.191.137.168:6080/arcgis/rest/services/BaseMap/GISBASEMAPTEST/MapServer");
		//map.addLayer(layer);

        arrService.forEach(function (entry) {
			layerbaseMapID = entry.split('/');
            URL = entry;
            layer = new esri.layers.ArcGISDynamicMapServiceLayer(URL, {id : layerbaseMapID[layerbaseMapID.indexOf("MapServer") -1]});
            map.addLayer(layer);
        });
		
		
    }


    function checkSiblings($el, checked) {
        var parent = $el.parent().parent(),
            all = true,
            indeterminate = false;

        $el.siblings().each(function () {
            return all = ($(this).children('input[type="checkbox"]').prop("checked") === checked);
        });

        if (all && checked) {
            parent.children('input[type="checkbox"]')
            .prop({
                indeterminate: false,
                checked: checked
            })
            .siblings('label')
            .removeClass('custom-checked custom-unchecked custom-indeterminate')
            .addClass(checked ? 'custom-checked' : 'custom-unchecked');

            checkSiblings(parent, checked);
        }
        else if (all && !checked) {
            indeterminate = parent.find('input[type="checkbox"]:checked').length > 0;

            parent.children('input[type="checkbox"]')
            .prop("checked", checked)
            .prop("indeterminate", indeterminate)
            .siblings('label')
            .removeClass('custom-checked custom-unchecked custom-indeterminate')
            .addClass(indeterminate ? 'custom-indeterminate' : (checked ? 'custom-checked' : 'custom-unchecked'));

            checkSiblings(parent, checked);
        }
        else {
            $el.parents("li").children('input[type="checkbox"]')
            .prop({
                indeterminate: true,
                checked: false
            })
            .siblings('label')
            .removeClass('custom-checked custom-unchecked custom-indeterminate')
            .addClass('custom-indeterminate');
        }
    }
});