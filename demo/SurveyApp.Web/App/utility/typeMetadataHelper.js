//for full functionality requires the following loaded in the global namespace
// • knockout.js
// • koockout.mapping plugin
// • jQuery
// • knockout.validation plugin (for applyValidation only)
define(function () {
    return {
        configure: function (typeMetadatasource, typeFieldName /*if not using JSON.NET*/) {
            /// <summary>must be defined at JavaScript app start as the result of a C# TypeMetadataHelper.EmitTypeMetadataArray() call</summary>
            /// <param name="typeMetadataource" type="Object"></param>

            this.allTypeMetadata = typeMetadatasource;
            
            if(typeFieldName)
                this.typeFieldName = typeFieldName;

            validateConfiguration(this);

            if (ko && ko.validation)
                wireUpAdditionalValidationRules();
        },
        
        allTypeMetadata: null, //must be defined, should be a type resulting from a a C# TypeMetadataHelper.EmitTypeMetadataArray() call
        
        typeFieldName: "$type", //JSON.NET default, servicestack is __type
        
        getTypeName: function (object) {
            /// <summary>determine the type name given an object. EX: $type='Foo.Bar.Baz, Foo'   will be   'Foo.Bar.Baz'</summary>
            /// <param name="object" type="Object"></param>

            if (!object)
                throw "object was null or not specified!";

            if (!object[this.typeFieldName])
                throw "type not specified in object!";

            var result = ko.utils.unwrapObservable(object[this.typeFieldName]);

            if (!result)
                throw "error parsing type information";

            return result;
        },

        isType: function (object, typeNameContains) {
            /// <summary>is the passed object's type the passed typeName?</summary>
            /// <param name="object" type="Object"></param>
            /// <param name="typeNameContains" type="Object"></param>

            return this.getTypeName(object).toLowerCase().indexOf(typeNameContains.toLowerCase()) !== -1;
        },
        
        getMetadata: function (typeNameContains) {
            /// <summary>get metatadata for a passed object typeName</summary>
            /// <param name="typeName" type="Object"></param>

            var self = this;
            validateConfiguration(self);
            
            var typeMetadata = ko.utils.arrayFirst(self.allTypeMetadata, function (metadata) {
                return metadata.TypeName.toLowerCase().indexOf(typeNameContains.toLowerCase()) !== -1;
            });
            
            if (!typeMetadata)
                throw "Type " + typeName + " could not be found in typeMetadata. Is it not defined?";

            return typeMetadata;
        },

        getInstance: function (typeNameContains) {
            var metaData = this.getMetadata(typeNameContains);
            metaData.Instance[this.typeFieldName] = metaData.TypeName;
            return metaData.Instance;
        },
        
        createAndAssignType: function (typeNameContains, referenceToAssign) {
            /// <summary>lookup the passed typeName in the configured typeMetadata source and assign it with ko.mapping to the reference</summary>
            /// <param name="typeName" type="Object"></param>
            /// <param name="referenceToAssign" type="Object"></param>

            validateConfiguration(this);

            var self = this;

            var oldtypeMetadata = ko.utils.arrayFirst(self.allTypeMetadata, function (metadata) {
                return self.isType(referenceToAssign, metadata.TypeName);
            });

            var metadata = self.getMetadata(typeNameContains);
            var instance = metadata.Instance;
            
            //first, use ko.mapping to create/update observables fields on the reference in place
            //don't do the type until we are all done since many observables may be dependent on type
            var settings = {
                ignore: [self.typeFieldName]
            };
            ko.mapping.fromJS(instance, settings, referenceToAssign);
            
            //second, delete any fields on 'referenceToAssign' that are in the old template but not in the new
            //not strictly necessary but reduces over the wire garbage that would be thrown out by JSON deserializer anyways..
            for (var field in oldtypeMetadata.Instance) {
                if (instance[field] === undefined)
                    delete referenceToAssign[field];
            }
            
            //finally change the type on the object to the new type and fix mapping to recognize it on ko.mapping.toJSON
            referenceToAssign[self.typeFieldName](metadata.TypeName);
            referenceToAssign.__ko_mapping__.ignore.splice(self.typeFieldName);
        },
        
        //REQUIRES knockout.validation plugin in global namespace
        applyValidation: function(object) {
            /// <summary>recursively apply validation defined in typeMetadata to an entire object graph</summary>
            /// <param name="objectGraph" type="Object">an object graph to apply validation to</param>

            var self = this;

            if (!object || !object[this.typeFieldName])
                return; //no type info here...

            var typeMetadata = this.getMetadata(this.getTypeName(object));

            if (!typeMetadata || !typeMetadata.FieldValidationRules)
                return;

            for (var fieldName in object) {
                var fieldObservable = object[fieldName];
                parseAndApplyValidations(fieldName, fieldObservable, typeMetadata);

                var fieldValue = ko.utils.unwrapObservable(fieldObservable);
                
                if (!fieldValue)
                    continue;
                
                if (typeof fieldValue == "object")
                    self.applyValidation(fieldValue); //continue to recurse down...
                
                if ($.isArray(fieldValue))
                    //loop across array adding validation for each item
                    $.each(fieldValue, function(idx, val) {
                        var obj = ko.utils.unwrapObservable(val);
                        self.applyValidation(obj);
                    });
            }
        }
    };
    
    function validateConfiguration(typeMetadataHelper) {
        
        if (!jQuery || !ko || !ko.mapping)
            throw "typeMetadataHelper doesn't have its required dependencies of jQuery, Knockout.js and knockout.mapping";

        if (!typeMetadataHelper.allTypeMetadata || !jQuery.isArray(typeMetadataHelper.allTypeMetadata))
            throw "typeMetadataHelper must be configured with the source of typeMetadata at Javascript App start. This is usually the result of a C# TypeMetadataHelper.EmitTypeMetadataArray() call";
    }
    
    function parseAndApplyValidations(fieldName, fieldObservable, typeMetadata, object) {
        
        if (!ko.isObservable(fieldObservable))
            return; //can't apply knockout.validation to a non observable field

        var fieldValidationRule = ko.utils.arrayFirst(typeMetadata.FieldValidationRules, function (field) {
            return field.FieldName === fieldName;
        });
        
        if (!fieldValidationRule)
            return;


        //data annotations
        applyIfHasRule("Required", function (rule) {
            wireUpRuleWithPossibleMessageOverride(rule, 'required', true);
        });
        applyIfHasRule("Range", function (rule) {
            wireUpRuleWithPossibleMessageOverride(rule, 'min', rule.Minimum);
            wireUpRuleWithPossibleMessageOverride(rule, 'max', rule.Maxmium);
        });
        applyIfHasRule("MinLength", function (rule) {
            wireUpRuleWithPossibleMessageOverride(rule, 'minLength', rule.Length);
        });
        applyIfHasRule("MaxLength", function (rule) {
            wireUpRuleWithPossibleMessageOverride(rule, 'maxLength', rule.Length);
        });
        applyIfHasRule("Regex", function (rule) {
            wireUpRuleWithPossibleMessageOverride(rule, 'pattern', rule.Pattern);
        });
        applyIfHasRule("EmailAddress", function (rule) {
            wireUpRuleWithPossibleMessageOverride(rule, 'email', true);
        });
        applyIfHasRule("Compare", function (rule) {
            wireUpRuleWithPossibleMessageOverride(rule, 'equal', object[rule.OtherField]);
        });
        applyIfHasRule("Phone", function () {
            fieldObservable.extend({ phoneUS: true });
        });
        applyIfHasRule("Url", function () {
            fieldObservable.extend({ url: true });
        });
        

        //ints
        applyIfHasRule("Short", function () {
            fieldObservable.extend({ required: true });
            fieldObservable.extend({ 'short': true });
        });
        applyIfHasRule("NullableShort", function () {
            fieldObservable.extend({ 'short': true });
        });
        
        applyIfHasRule("Int", function () {
            fieldObservable.extend({ required: true });
            fieldObservable.extend({ 'int': true });
        });
        applyIfHasRule("NullableInt", function () {
            fieldObservable.extend({ 'int': true });
        });
        
        applyIfHasRule("Long", function () {
            fieldObservable.extend({ required: true });
            fieldObservable.extend({ 'long': true });
        });
        applyIfHasRule("NullableLong", function () {
            fieldObservable.extend({ 'long': true });
        });


        //floats
        applyIfHasRule("FloatingPoint", function () {
            fieldObservable.extend({ required: true });
            fieldObservable.extend({ number: true });
        });
        applyIfHasRule("NullableFloatingPoint", function () {
            fieldObservable.extend({ number: true });
        });
        

        //other
        applyIfHasRule("DateTime", function () {
            fieldObservable.extend({ required: true });
            fieldObservable.extend({ date: true });
        });
        applyIfHasRule("NullableDateTime", function () {
            fieldObservable.extend({ date: true });
        });
        
        function applyIfHasRule(ruleName, applyRuleFunc) {
            var theRule = ko.utils.arrayFirst(fieldValidationRule.Rules, function (rule) {
                return rule.Name === ruleName;
            });

            if (!theRule)
                return;

            applyRuleFunc(theRule);
        }
        
        function wireUpRuleWithPossibleMessageOverride(rule, koValidationRuleName, ruleValue) {
            if (!rule.ErrorMessage) {
                var ruleObj = {};
                ruleObj[koValidationRuleName] = ruleValue;
                fieldObservable.extend(ruleObj); //use built in validation & message
            } else
                //use built in logic but override with custom server specified message
                //by specifiying a "Single-Use Custom Rule" https://github.com/ericmbarnard/Knockout-Validation/wiki/Custom-Validation-Rules
                fieldObservable.extend({
                    validation: {
                        validator: function (value) {
                            return ko.validation.rules[koValidationRuleName].validator(value, ruleValue);
                        },
                        message: rule.ErrorMessage,
                    }
                });
        }
    }
    
    function wireUpAdditionalValidationRules(){

        ko.validation.rules['short'] = {
            validator: function (value, validate) {
                return !value || (validate && validateInt(value, 32767));
            },
            message: "The specified value must be an whole number between +/- 32,767"
        };
        
        ko.validation.rules['int'] = {
            validator: function (value, validate) {
                return !value || (validate && validateInt(value, 2147483648));
            },
            message: "The specified value must be an whole number between +/- 2,147,483,648"
        };
        
        ko.validation.rules['long'] = {
            validator: function (value, validate) {
                return !value || (validate && validateInt(value, 9223372036854775808));
            },
            message: "The specified value must be an whole number between +/- 9,223,372,036,854,775,808"
        };
        
        ko.validation.rules['url'] = {
            validator: function (value, validate) {
                //the built in MS one is probably copywritten or something...
                var expr = /[-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?/gi;
                var regex = new RegExp(expr);
                return !value || (validate && regex.test(value));
            },
            message: "The specified value must be a URL"
        };
        
        //allows for wierd non-int junk like commas but not .'s
        function validateInt(value, range) {
            if (!/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(value))
                return false;
            
            if(typeof value == "string") {
                value = value.replace(/[^0-9.]/g, "");
                value = parseFloat(value);
            }
            
            if (typeof value !== "number")
                return false;

            return value >= (-1 * range) &&
                    value <= range &&
                    value % 1 === 0;
        }

        ko.validation.registerExtenders();
    }
});