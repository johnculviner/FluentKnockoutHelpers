define(['typeTemplates'],
function (typeTemplates) {
    //assumes JSON.NET serializer, TypeTemplateFor<TechProduct> and ko.mapping use
    //look at type information to figure out what $type objects are
    //also assumes a 'define' of 'typeTemplates' using TypeTemplateFor<TechProduct>
    //to create new types
    return {
        isType: function (object, type) {
            if (!type)
                throw "type not specified!";

            return getFullTypeName(object).toLowerCase().indexOf(type.toLowerCase()) !== -1;
        },
        
        getTypeName: function (object) {
            return getFullTypeName(object);
        },
        
        createAndAssignType: function (typeName, referenceToAssign) {

            var type = ko.utils.arrayFirst(typeTemplates.types, function(type) {
                return type.TypeName === typeName;
            });
            
            if (!type)
                throw "Type " + typeName + "could not be found in typeTemplates. Is it not defined?";

            var instance = type.TemplateInstance;
            
            //first, use ko.mapping to create/update observables fields on the reference in place
            ko.mapping.fromJS(instance, {}, referenceToAssign);
            
            //second, delete any fields on 'referenceToAssign' that are not contained in TemplateInstance
            //for (var field in referenceToAssign) {
            //    if (instance[field] === undefined)
            //        delete referenceToAssign[field];
            //}
        }
    };
    
    function getFullTypeName(object) {
        if (!object)
            throw "object was null or not specified!";

        if (!object.$type)
            throw "type not specified in object!";

        var result = /^[^,]*/.exec(ko.utils.unwrapObservable(object.$type));
        
        if (!result)
            throw "error parsing type information";

        return result[0];
    }
});