define(['./relation', './techProduct'],
function (relation, techProduct) {
    return function (apiSurvey) {
        var self = this;
        thevm = self;


        //perform custom mappings for child fields of the C# type Survey here
        //otherwise fields will be recursively turned into observables automatically
        //we only do this on fields that we want to 'extend' the functionality of the .NET type

        var settings = {
            Children: {
                create: function (options) {
                    return new relation(options.data, self);
                }    
            },

            TechProducts: { //tech products have special functionality and display logic so we substitue our own .NET type
                create: function (options) {
                    return new techProduct(options.data /*this is the C# TechProduct*/);
                }
            }
        };

        ko.mapping.fromJS(apiSurvey, settings, self);
    };
});