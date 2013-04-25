define(['utility/typeMetadataHelper', 'durandal/app'],
function (typeMetadataHelper, app) {
    return function (apiFoodGroup) {
        var self = this;

        //create observables for the C# apiFoodGroup on this class
        ko.mapping.fromJS(apiFoodGroup, {}, self);
        
        //apply validation recursively 
        typeMetadataHelper.applyValidation(self);

        //#region Click Events
        self.addFood = function () {
            
            //get an instance of a C# (api) Food from the metaDatahelper
            //that is observable, validation enabled and ready to go...
            var apiFood = typeMetadataHelper.getMappedValidatedInstance('models.food,');

            //no need to extend a c# food's definition
            //it already has everything we need
            self.Foods.push(apiFood);
        };

        self.removeFood = function (foodToRemove) {
            app.showMessage(
                "Are you sure you want to delete '" + foodToRemove.Name() + "'?",   //message
                "Delete food?",                                                     //title
                ['Delete', 'Cancel']                                                //button options (first is default)
            )
            .then(function (result) {
                //this promise resolves with the above selection when the modal is closed
                if (result === 'Delete')
                    self.Foods.remove(foodToRemove);
            });
        };
        //#endregion
    };
});