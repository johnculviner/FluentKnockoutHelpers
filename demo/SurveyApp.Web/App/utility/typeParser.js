define(function () {
    //assuming JSON.NET serializer look at type information to figure out what
    //what type of object this is for display purposes
    return {
        isType: function (object, type) {

            validateObject(object);

            if (!type)
                throw "type not specified!";

            return object.$type().toLowerCase().indexOf(type.toLowerCase()) !== -1;
        },
        typeName: function (object) {
            validateObject(object);

            //bad solution
            //below is a combination of me and javascript both sucking at regex
            var almostTypeName = /^.*,/.exec(object.$type())[0];
            return almostTypeName.substring(almostTypeName.lastIndexOf('.') + 1).replace(',', '');
        }
    };
    
    function validateObject(object) {
        if (!object.$type)
            throw "type not specified in object!";

        if (!object)
            throw "object was null or not specified!";
    }
});