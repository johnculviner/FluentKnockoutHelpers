define(['async!https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false'],
function (/*maps API isn't an AMD*/) {
    var geocoder = new google.maps.Geocoder();

    //returns deferred which resolves on ajax complete
    return {
        search: function (searchTerm) {

            //setup deferred
            var deferred = $.Deferred();

            geocoder.geocode({ address: searchTerm }, function (results, status) {

                if (status == google.maps.GeocoderStatus.OK) {
                    //map response into something more usable
                    //same signature as C# Location Class
                    var mapped = _.map(results, function (match) {
                        return {
                            FormattedLocation: match.formatted_address,
                            Latitude: match.geometry.location.lat(),
                            Longitude: match.geometry.location.lng(),
                            toString: function () { return this.FormattedLocation; }
                        };
                    });

                    //resolve promise for any .done subscribers
                    deferred.resolve(mapped);
                } else {
                    deferred.reject(status);
                }

            });

            //return promise, subscribers can 'listen' to deferred resolutions like above with   .then(function(arg) { /*use arg*/ })    for example
            return deferred.promise();
        }
    };
})