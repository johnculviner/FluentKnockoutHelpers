define(['utility/typeMetadataHelper'],
function (typeMetadataHelper) {
    return function (apiFoodGroup) {
        var self = this;

        //create observables for the C# apiFoodGroup on this class
        self = ko.mapping.fromJS(apiFoodGroup);

        self.addFood = function () {
            
            //get an instance of a C# (api) Food from the metaDatahelper
            //that is observable, validation enabled and ready to go...
            var apiFood = typeMetadataHelper.getMappedValidatedInstance('models.food');

            //no need to extend a c# food's definition
            //it already has everything we need
            this.foodGroups.push(apiFood);
        };

        self.removeFood = function(foodToRemove) {
            self.Foods.remove(foodToRemove);
        };
    };
});