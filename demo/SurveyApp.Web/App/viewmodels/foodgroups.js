define(['api/foodGroupApi', './foodGroups/foodGroup', 'utility/typeMetadataHelper'],
function (foodGroupApi, foodGroup, typeMetadataHelper) {
    return {
        //assigned in activate
        foodGroups: ko.observableArray(), 

        //called before the viewModel and the view are 'composed' (ko.applyBindings among other things)
        activate: function () {
            var self = this;
            //the viewModel and view aren't composed until this promise resolves
            return foodGroupApi.getAll()
                .then(function(apifoodGroups) {

                    //extend the C# definition of a food group with a JS class
                    var wrappedFoodGroups = ko.utils.arrayMap(apifoodGroups, function(apiFoodGroup) {
                        return new foodGroup(apiFoodGroup);
                    });

                    self.foodGroups(wrappedFoodGroups);
                });
        },
        
        addFoodGroup: function () {
            //get an instance of a C# (api) FoodGroup from the metaDatahelper
            var apiFoodGroupInstance = typeMetadataHelper.getInstance('models.foodgroup');

            //extend the C# definition of a food group with a JS class
            this.foodGroups.push(new foodGroup(apiFoodGroupInstance));
        },
        
        removeFoodGroup: function (foodGroupToRemove) {
            this.foodGroups.remove(foodGroupToRemove);
        }
    };
});