define(['durandal/app',
        'api/surveyApi',
        'api/foodGroupApi',
        './shared/locationInfo',
        'api/geocoderApi',
        'durandal/plugins/router',
        './survey',
        './addEditTechProductModal',
        'utility/typeMetadataHelper',
        './relation',
    
    //custom bindings (could also be loaded into 'gloabal namespace' as an alternative)
    'knockoutPlugins/bindingHandlers/autoComplete', 'knockoutPlugins/bindingHandlers/datepicker'],
function (app, surveyApi, foodGroupApi, locationInfo,
    geocoderApi, router, survey, addEditTechProductModal, typeMetadataHelper, relation) {

    return function () {

        var self = this;

        window.thevm = self;

        //assigned in activate
        self.survey = null;
        self.foodGroups = [];
        self.isNew = false;

        //the router's activator calls this function and waits for the .complete jQuery Promise before
        //transitioning the view in and eventually calling ko.applyBindings
        //routeInfo contains the passed 'id' as configured in main.js
        self.activate = function (routeInfo) {
            
            var foodGroupPromise =
                foodGroupApi.getAll()
                    .then(function(foodGroups) {
                        self.foodGroups = foodGroups;
                        return foodGroups;
                    });

            var surveyDeferred = $.Deferred();

            //'#/survey/new' means new survey. get it from typeMetadata. otherwise retreieve from the API
            self.isNew = routeInfo.id == 'new';

            if (self.isNew) {
                //NEW SURVEY

                self.survey = new survey(typeMetadataHelper.getInstance('models.survey,'), foodGroupPromise);
                
                //only hack in the whole demo!:
                //for dirtyFlag to work this needs to be done because knockout
                //will set dropdown value fields to 'undefined' when it can't
                //find the value in the select list which will break dirty handling for new surveys
                if (!self.survey.FavoriteColorId())
                    self.survey.FavoriteColorId(undefined);

                //apply validation to the entire model and object graph using metadata from C#
                //TypeMetadataHelper.EmitTypeMetadataArray()
                typeMetadataHelper.applyValidation(self.survey);
                
                surveyDeferred.resolve(); //resolve immediately as no AJAX is required...
            }
            else
                //EXISTING SURVEY
                surveyApi.get(routeInfo.id)
                    .then(function (apiSurvey) {

                        //extend the c# definition of a survey for a good UI experience
                        self.survey = new survey(apiSurvey, foodGroupPromise);
                        
                        //apply validation to the entire model and object graph using metadata from C#
                        //TypeMetadataHelper.EmitTypeMetadataArray()
                        typeMetadataHelper.applyValidation(self.survey);

                        surveyDeferred.resolve();
                    });
            
            //the promise resolves 'when' the above promises complete.
            //the promising resolving allows durandal to go ahead with composition
            //because the view model is 'ready'
            return $.when(surveyDeferred.promise(), foodGroupPromise)
                        .then(ajaxLoaded);
        };
        

        //wire up computeds that need data populated to complete
        function ajaxLoaded() {
            
            //#region Computeds
            self.headerText = ko.computed(function() {

                var message = (self.isNew ? "New" : "Update") + " Survey";

                var firstNameVal = self.survey.FirstName();
                var lastNameVal = self.survey.LastName();

                if (firstNameVal && lastNameVal) {
                    message += " for: " + lastNameVal + ", " + firstNameVal;
                }
                return message;
            }); 

            self.saveText = ko.computed(function() {
                return (self.isNew ? "Create New" : "Update");
            });

            //#region Save/Cancel 
            self.dirtyFlag = new ko.DirtyFlag(self.survey, false, ko.mapping.toJSON);   //kolite plugin
            self.validator = ko.validatedObservable(self.survey);          //knockout validation plugin


            self.save = ko.asyncCommand({ //kolite plugin
                execute: function () {
                    surveyApi.post(ko.mapping.toJS(self.survey))
                        .then(function () {
                            self.dirtyFlag().reset();
                            router.navigateTo('#/surveys');
                        });
                },
                canExecute: function (isExecuting) {
                    return !isExecuting && self.dirtyFlag().isDirty() && self.validator().isValid();
                }
            });

            var resetCopy = ko.mapping.toJS(self.survey);
            self.reset = function () {
                ko.mapping.fromJS(resetCopy, {}, self.survey);
            };

            self.cancel = function () {
                router.navigateTo('#/surveys');
            };
            //#endregion
        }


        //user is trying to transition to another view.
        //lets not allow this without a confirm if there are unsaved changes
        self.canDeactivate = function () {
            
            if (!self.isDirty())
                return true;

            return app.showMessage(
                "If you leave this page without saving you will lose your changes!",    //message
                "Leave page without saving?",                                           //title
                ['Cancel', 'Leave']                                  //buttons, (first is default)
            ).then(function (resp) {
                //this promise resolves with the above selection when the modal is closed
                return resp === 'Leave';
            });
        };


        //remaining non-depedent functions

        //#region Home Location Autocomplete
        //query google maps for address
        //this is more coupled to 'jquery autocomplete' than it should be and could be moved into an elaborate plugin
        self.getMatchingLocations = function (request, response) {

            geocoderApi.search(request.term)
                .done(function (locations) {

                    var resp = $.map(locations, function (location) {
                        location.toString = function() { return this.FormattedLocation; }; //hack for bug in jQuery UI
                        return {
                            label: location.FormattedLocation,
                            value: location
                        };
                    });
                    //jquery autocomplete response ..ick
                    response(resp);
                });
        };

        //this is more coupled to 'jquery autocomplete' than it should be and could be moved into an elaborate plugin
        self.selectLocation = function (e, ui) {
            delete ui.item.value.toString; //remove tostring hack for bug in jQuery UI
            ko.mapping.fromJS(ui.item.value, {}, self.survey.HomeLocation);
            e.target.value = "";
            return false;
        };
        //#endregion


        self.addChild = function () {
            //get an instance of this C# type from typeMetaDataHelper
            //wrap it in THIS custom javascript object.
            var newChildRelation = new relation(typeMetadataHelper.getInstance('relation'), self.survey);
            self.survey.Children.push(newChildRelation);
        };

        //#region Tech Product CRUD
        self.addTechProduct = function () {
            app.showModal(new addEditTechProductModal(null, self.survey))
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

            app.showModal(new addEditTechProductModal(copy, self.survey))
                .then(function(modifiedTechProduct) {
                    if (modifiedTechProduct)
                        modifiedTechProduct.merge();
                });
        };
        
        self.deleteTechProduct = function (techProduct) {
            app.showMessage(
                "Are you sure you want to delete this " + techProduct.productTypeDisplay() + "?",   //message
                "Delete tech product?",                                                             //title
                ['Delete', 'Cancel']                                      //button options (first is default)
            )
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