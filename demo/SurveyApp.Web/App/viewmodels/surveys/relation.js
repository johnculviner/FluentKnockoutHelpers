define(['durandal/app', 'utility/typeMetadataHelper'],
function (app, typeMetadataHelper) {
    var relation = function (apiRelation, parent) {
        var self = this;
        this.parent = parent;

        //recursively handle creating relations
        var settings = {
            Children: {
                create: function (options) {

                    if (!options.data)                  //this is the C# Relation
                        return ko.observableArray([]);  //empty array to add more children later maybe...

                    return new relation(options.data , self);
                }
            }
        };

        ko.mapping.fromJS(apiRelation, settings, self);
        

        self.addChild = function () {

            //get an instance of this C# type from typeMetaDataHelper
            //wrap it in THIS custom javascript object.
            var newChildRelation = new relation(typeMetadataHelper.getInstance(self.$type), self);
            self.Children.unshift(newChildRelation);
        };

        self.removeChild = function () {
            app.showMessage(
                "Are you sure you want to delete '" + self.Name() + "' any descendants?",   //message
                "Delete " + self.Name() + "?",                                              //title
                ['Delete', 'Cancel']                                                        //buttons, (first is default)
            ).then(function (resp) {
                //the promise returns the selection
                if (resp === 'Delete')
                    self.parent.Children.remove(self);
            });
        };
    };

    return relation;
});