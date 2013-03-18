define(['googleMapsApi'],
function () {
    return function() {
        var self = this;

        var geocoder = new google.maps.Geocoder();

        //returns deferred which resolves on ajax complete
        self.search = function(searchTerm) {

            //setup deferred
            var deferred = $.Deferred();

            geocoder.geocode({ address: searchTerm }, function(results) {

                //map response into something more usable
                var mapped = _.map(results, function(value) {
                    return {
                        address: value.formatted_address,
                        latitude: item.geometry.location.lat(),
                        longitude: item.geometry.location.lng()
                    };
                });

                //resolve promise for any subscribers
                deferred.resolve(mapped);
            });

            //return promise, subscribers can 'listen' to deferred resolutions like above with   .then(function(arg) { /*use arg*/ })    for example
            return deferred.promise();
        };
    };
})