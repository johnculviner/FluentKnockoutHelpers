define(function () {
    //assumes TypeTemplateFor<TType> and ko.mapping use
    //look at type information to figure out what $type objects are
    //typeTemplates must be set below
    //to create new types
    return {
        configure: function (typeTemplatesSource, typeFieldName /*if not using JSON.NET*/) {
            /// <summary>must be defined at app start, the result of a C# this.TypeTemplateFor&lt;TType&gt;() call</summary>
            /// <param name="typeTemplatesSource" type="Object"></param>

            this.typeTemplates = typeTemplatesSource;
            
            if(typeFieldName)
                this.typeFieldName = typeFieldName; 
        },
        
        typeTemplates: null, //must be defined, should be a type resulting from a TypeTemplateFor<TType> call
        
        typeFieldName: "$type", //JSON.NET default
        
        getTypeName: function (object) {
            /// <summary>determine the type name given an object. EX: $type='Foo.Bar.Baz, Foo'   will be   'Foo.Bar.Baz'</summary>
            /// <param name="object" type="Object"></param>

            if (!object)
                throw "object was null or not specified!";

            if (!object[this.typeFieldName])
                throw "type not specified in object!";

            var result = /^[^,]*/.exec(ko.utils.unwrapObservable(object[this.typeFieldName]));

            if (!result)
                throw "error parsing type information";

            return result[0];
        },

        isType: function (object, typeName) {
            /// <summary>is the passed object's type the passed typeName?</summary>
            /// <param name="object" type="Object"></param>
            /// <param name="typeName" type="Object"></param>

            return this.getTypeName(object).toLowerCase().indexOf(typeName.toLowerCase()) !== -1;
        },
        
        getTemplate: function (typeName) {
            var self = this;
            validateConfiguration(this);
            
            var typeTemplate = ko.utils.arrayFirst(this.typeTemplates.Templates, function (template) {
                return self.isType(template.TemplateInstance, typeName);
            });
            
            if (!typeTemplate)
                throw "Type " + typeName + "could not be found in typeTemplates. Is it not defined?";

            return typeTemplate;
        },

        getTemplateInstance: function (typeName) {
            return this.getTemplate(typeName).TemplateInstance;
        },
        
        createAndAssignType: function (typeName, referenceToAssign) {
            /// <summary>lookup the passed typeName in the configured typeTemplates source and assign it with ko.mapping to the reference</summary>
            /// <param name="typeName" type="Object"></param>
            /// <param name="referenceToAssign" type="Object"></param>

            validateConfiguration(this);

            var self = this;

            var oldTypeTemplate = ko.utils.arrayFirst(self.typeTemplates.Templates, function (template) {
                return self.isType(referenceToAssign, template.TypeName);
            });

            var newTypeTemplate = self.getTemplate(typeName);

            var instance = newTypeTemplate.TemplateInstance;
            
            //first, use ko.mapping to create/update observables fields on the reference in place
            //don't do the type until we are all done since many observables may be dependent on type
            var settings = {
                ignore: [self.typeFieldName]
            };
            ko.mapping.fromJS(instance, settings, referenceToAssign);
            
            //second, delete any fields on 'referenceToAssign' that are in the old template but not in the new
            //not strictly necessary but reduces over the wire garbage that would be thrown out by JSON deserializer anyways..
            for (var field in oldTypeTemplate.TemplateInstance) {
                if (newTypeTemplate.TemplateInstance[field] === undefined)
                    delete referenceToAssign[field];
            }
            
            //finally change the type on the object to the new type and fix mapping to recognize it on ko.mapping.toJSON
            referenceToAssign[self.typeFieldName](instance[self.typeFieldName]);
            referenceToAssign.__ko_mapping__.ignore.splice(self.typeFieldName);
        }
    };
    
    function validateConfiguration(typeHelper) {
        if (!typeHelper.typeTemplates || !$.isArray(typeHelper.typeTemplates.Templates))
            throw "typeTemplates must be set to the source of type templates at app start. This is usually the result of a C# this.TypeTemplateFor<TType>() call";
    }
});