define(['api/surveyApi', './shared/locationInfo', 'async!https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false', 'knockoutPlugins/bindingHandlers/autoComplete'],
function (surveyApi, locationInfo) {

    return function (surveySummary) {

        var self = this;

        self.survey = null; //assigned in activate

        //the router's activator calls this function and waits for the .complete jQuery Promise before proceding
        self.activate = function () {
            return surveyApi.get(surveySummary.SurveyId())
                        .then(function (fullSurvey) {

                            //use ko.mapping library to convert the entire survey to an observable
                            self.survey = ko.mapping.fromJS(fullSurvey);
                        });
        };
        
        //#region Autocomplete
        //query google maps for address
        //this is more coupled to 'jquery autocomplete' than it should be and could be moved into an elaborate plugin
        self.getMatchingLocations = function(request, response) {

            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ address: request.term }, function(results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    response(_.map(results, function(match) {

                        //C# Class Location
                        var val = {
                            FormattedLocation: match.formatted_address,
                            Latitude: match.geometry.location.lat(),
                            Longitude: match.geometry.location.lng()
                        };
                        val.toString = function() { return this.FormattedLocation; }; //aformentioned jquery autocomplete coupling

                        return {
                            label: match.formatted_address,
                            value: val
                        };
                    }));
                }
            });
        };

        //this is more coupled to 'jquery autocomplete' than it should be and could be moved into an elaborate plugin
        self.selectLocation = function(e, ui) {
            self.survey.Location(ui.item.value);
        };
        //#endregion

        self.close = function () {
            this.modal.close(null);
        };
        
        //for locationInfo compose
        self.locationInfo = locationInfo;
    };
});