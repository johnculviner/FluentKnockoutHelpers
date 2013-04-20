define(['durandal/app',
        'api/surveyApi',
        'api/colorApi',
        './shared/locationInfo',
        'api/geocoderApi',
        'durandal/plugins/router',
        './survey',
        './addEditTechProductModal',
        'utility/typeMetadataHelper',
    
    //custom bindings    
    'knockoutPlugins/bindingHandlers/autoComplete', 'knockoutPlugins/bindingHandlers/datepicker'],
function (app, surveyApi, colorApi, locationInfo, geocoderApi, router, survey, addEditTechProductModal, typeMetadataHelper) {

    return function () {

        var self = this;

        //assigned in activate
        self.survey = null;
        self.colors = [];

        self.isNew = ko.observable(false);

        //the router's activator calls this function and waits for the .complete jQuery Promise before
        //transitioning the view in and eventually calling ko.applyBindings
        //routeInfo contains the passed 'id' as configured in main.js
        self.activate = function (routeInfo) {
            
            //'#/survey/new' means new survey. get it from typeMetadata
            if (routeInfo.id == 'new') {
                //TODO:
            }

            //get the survey from the API
            var surveyDeferred =
                surveyApi.get(routeInfo.id)
                    .then(function (apiSurvey) {

                        //extend the c# definition of a survey for a good UI experience
                        //inside survey class. (Also just does a ko.mapping on it)
                        self.survey = new survey(apiSurvey);
                        
                        //apply validation to the entire model and object graph using metadata from C#
                        //TypeMetadataHelper.EmitTypeMetadataArray()
                        typeMetadataHelper.applyValidation(self.survey);
                    });
            
            //load color dropdown from the API
            var colorsDeferred =
                colorApi.getAll()
                    .then(function(colors) {
                        self.colors = colors;
                    });


            //the promise resolves 'when' the above deferreds complete.
            //the promising resolving allows durandal to go ahead with composition
            //because the view model is 'ready'
            return $.when(surveyDeferred, colorsDeferred);
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
            ko.mapping.fromJS(ui.item.value, {}, self.survey.HomeLocation);
            e.target.value = "";
            return false;
        };
        //#endregion


        //#region Computeds
        self.headerText = ko.computed(function() {

            var message = (self.isNew() ? "New" : "Update") + " Survey";

            var firstNameVal = self.survey.FirstName();
            var lastNameVal = self.survey.LastName();

            if (firstNameVal && lastNameVal) {
                message += " for: " + lastNameVal + ", " + firstNameVal;
            }

            return message;
            //wait until first access of computed for eval. otherwise eval would occur *immediately* on object creation
            //which is before the data is loaded from ajax
        }, this, { deferEvaluation: true }); 

        self.saveText = ko.computed(function() {
            return (self.isNew() ? "Create New" : "Update");
        });


        self.selectedColorObj = ko.computed(function () {
            return ko.utils.arrayFirst(self.colors, function (c) {
                return c.ColorId == self.survey.FavoriteColorId();
            }) || { };
        }, this, { deferEvaluation: true });


        //#endregion


        //#region Save/Cancel Events
        self.save = function() {
            surveyApi.post(ko.mapping.toJS(self.survey))
                .done(function() {
                    router.navigateTo('survey');
                });
        };

        self.cancel = function () {
            router.navigateTo('survey');
        };
        //#endregion
        

        //#region Tech Product CRUD
        self.addTechProduct = function () {
            app.showModal(new addEditTechProductModal())
                .then(function (newTechProduct) {
                    //this promise resolves when the modal is closed
                    
                    if(newTechProduct) //didn't click 'cancel'
                        self.survey.TechProducts.push(newTechProduct);
                });
        };
        
        self.editTechProduct = function (techProduct) {
            
            //create a copy of the current tech product to enable cancelling
            //if a copy wasn't made then the observables in the tech product list
            //would reflect instantly as a user made changes in the modal. might freak them out...
            var copy = typeMetadataHelper.copy(techProduct);

            app.showModal(new addEditTechProductModal(copy))
                .then(function(modifiedTechProduct) {
                    if (modifiedTechProduct)
                        modifiedTechProduct.merge();
                });
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

        
        //for locationInfo compose
        self.locationInfo = locationInfo;
    };
});