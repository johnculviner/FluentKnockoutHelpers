define(['http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0'],
function (/*maps API isn't an AMD*/) {
    
    //a simple binding for an embedded bing map on an element bound to an observable
    //implicitly requires jQuery
    ko.bindingHandlers.bingMap = {

        init: function (element, valueAccessor) {
            
            var location = getLatLngForObservable(valueAccessor());

            // create the map
            var map = new Microsoft.Maps.Map(element,
                {
                    credentials: 'Annwexqz0WWEN7ydadyeGs3rVi3RdkiCI_XzR-NuLHLvLe2LHPp55oPBMcNG70Ir', //don't steal my creds bro
                    center: location,
                    zoom: 6,
                    mapTypeId: Microsoft.Maps.MapTypeId.road,
                    showMapTypeSelector: false
                });

            var pushPin = dropPushPin(location, map);

            $(element).data('mapData', { map: map, pushPin: pushPin });
        },

        update: function (element, valueAccessor) {

            var location = getLatLngForObservable(valueAccessor());

            var mapData = $(element).data('mapData');

            if (mapData && mapData.map) {
                
                if (mapData.pushPin)
                    mapData.map.entities.remove(mapData.pushPin);
                
                mapData.map.setView({center: location});
                var pushPin = dropPushPin(location, mapData.map);
                
                $(element).data('mapData', { map: mapData.map, pushPin: pushPin });
            }
        }
    };
    
    function dropPushPin(location, map) {
        var pushPin = new Microsoft.Maps.Pushpin(location);
        map.entities.push(pushPin);
        return pushPin;
    }


    function getLatLngForObservable(value) {
        return new Microsoft.Maps.Location(ko.utils.unwrapObservable(value.latitude), ko.utils.unwrapObservable(value.longitude));
    }

})