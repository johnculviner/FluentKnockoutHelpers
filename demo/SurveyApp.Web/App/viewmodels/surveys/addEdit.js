define(['durandal/app', 'api/surveyApi', './shared/locationInfo', 'api/geocoderApi', 'durandal/plugins/router', './survey', './addEditTechProductModal',
    //custom bindings    
    'knockoutPlugins/bindingHandlers/autoComplete', 'knockoutPlugins/bindingHandlers/datepicker'],
function (app, surveyApi, locationInfo, geocoderApi, router, survey, addEditTechProductModal) {

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
                        .then(function (apiSurvey) {

                            //do some custom stuff inside survey class in addition to doing a ko.mapping on it
                            self.survey = new survey(apiSurvey);
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
            ko.mapping.fromJS(ui.item.value, {}, self.survey.FavoritePlace);
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
            surveyApi.post(ko.mapping.toJS(self.survey))
                .done(function() {
                    router.navigateTo('surveys');
                });
        };

        self.cancel = function () {
            router.navigateTo('surveys');
        };
        

        //#region Tech Product
        self.addTechProduct = function () {
            app.showModal(new addEditTechProductModal())
                .then(function (newTechProduct) {
                    //this promise resolves when the modal is closed
                    self.survey.TechProducts.push(newTechProduct);
                });
        };
        
        self.editTechProduct = function(techProduct) {
            app.showModal(new addEditTechProductModal(techProduct));
        };
        
        self.deleteTechProduct = function (techProduct) {
            app.showMessage("Are you sure you want to delete this " + techProduct.productTypeDisplay() + "?",
                "Delete tech product?", ['Delete', 'Cancel'])
                .then(function(result) {
                    //this promise resolves with the above selection when the modal is closed
                    if(result === 'Delete')
                        self.survey.TechProducts.remove(techProduct);
                });
        };
        //#endregion


        //#endregion
        
        //for locationInfo compose
        self.locationInfo = locationInfo;
    };
});