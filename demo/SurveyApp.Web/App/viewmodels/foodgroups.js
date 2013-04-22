define(['api/foodGroupApi', './foodGroups/foodGroup', 'utility/typeMetadataHelper', 'durandal/app', 'durandal/plugins/router'],
function (foodGroupApi, foodGroup, typeMetadataHelper, app, router) {
    return new function() {
        var self = this;

        window.thevm = self;

        //assigned in activate
        self.foodGroups = ko.observableArray();

        //called before the viewModel and the view are 'composed' by durandal (ko.applyBindings among other things)
        self.activate = function() {
            //the viewModel and view aren't composed by durandal until this promise resolves
            return foodGroupApi.getAll()
                .then(function(apifoodGroups) {

                    //extend the C# definition of a food group with a JS class
                    var wrappedFoodGroups = ko.utils.arrayMap(apifoodGroups, function(apiFoodGroup) {
                        return new foodGroup(apiFoodGroup);
                    });

                    self.foodGroups(wrappedFoodGroups);
                    ajaxLoaded();
                });
        };

        self.addFoodGroup = function() {
            //get an instance of a C# (api) Food from the metaDatahelper
            //that is observable, validation enabled and ready to go...
            var apiFoodGroupInstance = typeMetadataHelper.getMappedValidatedInstance('models.foodgroup,');

            //extend the C# definition of a food group with a JS class
            self.foodGroups.push(new foodGroup(apiFoodGroupInstance));
        };
        
        self.removeFoodGroup = function (foodGroupToRemove) {
            app.showMessage(
                "Are you sure you want to delete '" + foodGroupToRemove.Name() + "'?",      //message
                "Delete food group?",                                                       //title
                ['Delete', 'Cancel']                                                        //button options (first is default)
            )
            .then(function (result) {
                //this promise resolves with the above selection when the modal is closed
                if (result === 'Delete')
                self.foodGroups.remove(foodGroupToRemove);
            });
        };
        

        function ajaxLoaded() {
            self.dirtyFlag = new ko.DirtyFlag(self.foodGroups, false, ko.mapping.toJSON);       //kolite plugin

            ko.validation.init({ grouping: { deep: true } });
            self.validator = ko.validatedObservable(self.foodGroups());                         //knockout validation plugin

            self.save = ko.asyncCommand({ //kolite plugin
                execute: function () {
                    foodGroupApi.post(ko.mapping.toJS(self.foodGroups))
                        .then(function () {
                            self.dirtyFlag().reset();
                            router.navigateTo('#/surveys');
                        });
                },
                canExecute: function (isExecuting) {
                    return !isExecuting && self.dirtyFlag().isDirty() && self.validator().isValid();
                }
            });

            var resetCopy = ko.mapping.toJS(self.foodGroups);
            self.reset = function () {
                ko.mapping.fromJS(resetCopy, {}, self.foodGroups);
            };

            self.cancel = function () {
                router.navigateTo('#/surveys');
            };
        }
    };
});