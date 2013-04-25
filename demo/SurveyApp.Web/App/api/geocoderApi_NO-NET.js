define( //no internet
function (/*maps API isn't an AMD*/) {

    //returns deferred which resolves on ajax complete
    return {
        search: function (searchTerm) {

            var deferred = $.Deferred();

            deferred.resolve({
                FormattedLocation: "no internet",
                Latitude: 0,
                Longitude: 0
            });

            return deferred.promise();
        }
    };
})