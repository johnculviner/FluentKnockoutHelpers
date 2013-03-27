define(['api/surveyApi', './shared/locationInfo', 'api/geocoderApi', 'durandal/plugins/router',
    //custom bindings    
    'knockoutPlugins/bindingHandlers/autoComplete', 'knockoutPlugins/bindingHandlers/datepicker'],
function (surveyApi, locationInfo, geocoderApi, router) {

    return function () {

        var self = this;

        self.survey = null; //assigned in activate
        self.isNew = ko.observable(false);

        //the router's activator calls this function and waits for the .complete jQuery Promise before
        //transitioning the view in and eventually calling ko.applyBindings
        //routeInfo contains the passed 'id' as configured in main.js
        self.activate = function (routeInfo) {
            
            //'#/surveys/new' means new survey. this maps to a '-1' id on the API which knows to return a blank template
            //for a new API as required to use ko.mapping. Another option would be to include these 'object templates' in
            //a require module at application start.
            if(routeInfo.id == 'new') {
                self.isNew = true;
                routeInfo.id = -1;
            }

            return surveyApi.get(routeInfo.id)
                        .then(function (survey) {

                            //use ko.mapping library to convert the entire survey to an observable
                            self.survey = ko.mapping.fromJS(survey);
                        });
        };
        
        //#region Autocomplete
        //query google maps for address
        //this is more coupled to 'jquery autocomplete' than it should be and could be moved into an elaborate plugin
        self.getMatchingLocations = function(request, response) {

            geocoderApi.search(request.term)
                .done(function(locations) {

                    var resp = $.map(locations, function (location) {
                        return {
                            label: location.FormattedAddress,
                            value: location
                        };
                    });
                    //jquery autocomplete response ..ick
                    response(resp);
                });
        };

        //this is more coupled to 'jquery autocomplete' than it should be and could be moved into an elaborate plugin
        self.selectLocation = function (e, ui) {
            ko.mapping.fromJS(ui.item.value, {}, self.survey.Location);
            e.target.value = "";
            return false;
        };
        //#endregion

        self.headerText = ko.computed(function() {

            var message = (self.isNew() ? "New" : "Update") + " Survey";

            var firstNameVal = self.survey.FirstName();
            var lastNameVal = self.survey.LastName();

            if (firstNameVal && lastNameVal) {
                message += " for: " + lastNameVal + ", " + firstNameVal;
            }

            return message;
        }, this, { deferEvaluation: true }); //wait until first access of computed for eval. otherwise eval would occur *immediately* on object creation

        self.saveText = ko.computed(function() {
            return (self.isNew() ? "Create New" : "Update");
        });


        //#region Click Events
        self.save = function() {
            //surveyApi.
        };

        self.cancel = function () {
            router.navigateTo('surveys');
        };
        //#endregion
        
        //for locationInfo compose
        self.locationInfo = locationInfo;
    };
});