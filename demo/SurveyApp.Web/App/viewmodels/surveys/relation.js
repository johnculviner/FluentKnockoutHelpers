define(['utility/typeMetadataHelper'],
function (typeMetadataHelper) {
    var relation = function (apiRelation, parent) {
        var self = this;
        this.parent = parent;

        //recursively handle creating relations
        var settings = {
            Children: {
                create: function (options) {

                    if (!options.data) /*this is the C# Relation*/
                        return ko.observableArray([]);

                    return new relation(options.data , self);
                }
            }
        };

        ko.mapping.fromJS(apiRelation, settings, self);
    };
    
    return relation;
});