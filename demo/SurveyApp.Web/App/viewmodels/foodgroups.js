define(['api/foodGroupApi', './foodGroups/foodGroup', 'utility/typeMetadataHelper',
        'durandal/app', 'durandal/plugins/router'],
function (foodGroupApi, foodGroup, typeMetadataHelper, app, router) {
    return new function() {
        var self = this;

        //assigned in activate
        self.foodGroups = ko.observableArray();

        //called before the viewModel and the view are
        //'composed' by durandal (ko.applyBindings among other things)
        self.activate = function() {
            //the viewModel and view aren't composed by durandal until this promise resolves
            return foodGroupApi.getAll()
                .then(function(apifoodGroups) {

                    //extend the C# definition of a food group with a JS class
                    var wrappedFoodGroups = ko.utils.arrayMap(apifoodGroups, function(apiFoodGroup) {
                        return new foodGroup(apiFoodGroup);
                    });

                    self.foodGroups(wrappedFoodGroups);
                });
        };

        self.addFoodGroup = function() {
            //get an instance of a C# (api) Food from the metaDatahelper
            //that is observable, validation enabled and ready to go...
            var apiFoodGroupInstance =
                typeMetadataHelper.getMappedValidatedInstance('models.foodgroup,');

            //extend the C# definition of a food group with a JS class
            self.foodGroups.push(new foodGroup(apiFoodGroupInstance));
        };
        
        self.removeFoodGroup = function (foodGroupToRemove) {
            app.showMessage(
                "Are you sure you want to delete '" + foodGroupToRemove.Name() + "'?",  //message
                "Delete food group?",                                                   //title
                ['Delete', 'Cancel']                                                    //button options (first is default)
            )
            .then(function (result) {
                //this promise resolves with the above selection when the modal is closed
                if (result === 'Delete')
                self.foodGroups.remove(foodGroupToRemove);
            });
        };
        
        self.save = function() {
            foodGroupApi.post(ko.mapping.toJS(self.foodGroups))
                .then(function() {
                    router.navigateTo('#/surveys');
                });
        };

        self.cancel = function () {
            router.navigateTo('#/surveys');
        };
    };
});