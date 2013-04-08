define(['./techProduct', 'utility/typeMetadataHelper'],
function (techProduct, typeMetadataHelper) {
    return function (apiSurvey) {
        var self = this;

        //perform custom mappings for the C# type Survey here
        //otherwise properties are just a 'pass through' using ko.mappings plugin
        //which will automagically create observables for everything on 'this'
        ko.mapping.fromJS(apiSurvey, {
            'TechProducts': {
                create: function (options) {
                    return new techProduct(options.data /*this is the C# TechProduct*/);
                }
            }
        }, self);

        //apply validation to the entire model and object graph using metadata from C# TypeMetadataHelper.EmitTypeMetadataArray()
        typeMetadataHelper.applyValidation(self);
        
        self.validator = ko.validatedObservable(self);
    };
});