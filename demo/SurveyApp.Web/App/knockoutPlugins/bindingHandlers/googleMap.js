define(['async!https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false'],
function (/*maps API isn't an AMD*/) {
    
    //a simple binding for an embedded google map on an element bound to an observable
    ko.bindingHandlers.googleMap = new function() {
        var map,
            marker;
        
        this.init = function (element, valueAccessor) {

            $(element).html('');

            var location = getLatLngForObservable(valueAccessor);
            
            map = new google.maps.Map(element, {
                zoom: 5,
                center: location,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                disableDefaultUI: true,
                draggable: false
            });
            
            dropMarker(location);
        };

        this.update = function (element, valueAccessor) {
            var location = getLatLngForObservable(valueAccessor);
            map.setCenter(location);
            dropMarker(location);
        };
        
        function getLatLngForObservable(valueAccessor) {
            var lat = ko.utils.unwrapObservable(ko.utils.unwrapObservable(valueAccessor()).latitude());
            var lng = ko.utils.unwrapObservable(ko.utils.unwrapObservable(valueAccessor()).longitude());

            return new google.maps.LatLng(lat, lng);
        }

        function dropMarker(location) {
            if (marker)
                marker.setMap(null);

            marker = new google.maps.Marker({
                position: location,
                map: map
            });
        }
    };
})