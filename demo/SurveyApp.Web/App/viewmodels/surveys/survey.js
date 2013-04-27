define(['./relation', './techProduct', 'api/surveyApi'],
function (relation, techProduct, surveyApi) {
    return function (apiSurvey, foodGroupPromise) {
        var self = this;

        //perform custom mappings for child fields of the C# type Survey here
        //otherwise fields will be recursively turned into observables automatically
        //we only do this on fields that we want to 'extend' the functionality of the .NET type
        var settings = {
            Children: {
                create: function (options) {
                    return new relation(options.data, self);
                }    
            },
            
            //tech products have special functionality and display logic so we substitue our own .NET type
            TechProducts: { 
                create: function (options) {
                    return new techProduct(options.data /*this is the C# TechProduct*/, self);
                }
            }
        };

        ko.mapping.fromJS(apiSurvey, settings, self);
        
        self.PersonIdNumber.extend({
            asyncValidation: function () {
                return surveyApi.validateIdNumberUnique(self.PersonIdNumber(), self.Id());
            }
        });


        //wire up food group, food, dependent drop down
        foodGroupPromise.then(function (foodGroups) {

            self.selectedFoodGroup = ko.observable();
            
            //select the correct foodgroup for displaying a previously created survey
            if (self.FavoriteFoodId()) {
                
                var selectedFoodGroup = ko.utils.arrayFirst(foodGroups, function (group) {
                    return ko.utils.arrayFirst(group.Foods, function (food) {
                        return food.Id === self.FavoriteFoodId();
                    }) != null;
                });

                self.selectedFoodGroup(selectedFoodGroup);
            }

            self.isFoodGroupSelected = ko.computed(function() {
                return self.selectedFoodGroup() != null;
            });

            self.availableFoods = ko.computed(function () {
                return self.isFoodGroupSelected() ? self.selectedFoodGroup().Foods : null;
            });
            
            return foodGroups;
        });
    };
});