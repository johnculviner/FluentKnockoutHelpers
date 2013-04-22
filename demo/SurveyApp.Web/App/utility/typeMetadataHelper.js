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

            if (!object || !object[this.typeFieldName])
                return null;

            var result = ko.utils.unwrapObservable(object[this.typeFieldName]);

            if (!result)
                return null;

            return result;
        },

        hasType: function (object) {
            return object && object[this.typeFieldName];
        },

        isType: function (object, typeNameContains) {
            /// <summary>is the passed object's type the passed typeName?</summary>
            /// <param name="object" type="Object"></param>
            /// <param name="typeNameContains" type="Object"></param>

            if (!object || !typeNameContains || !object[this.typeFieldName])
                return false;

            var typeName = this.getTypeName(object);
            
            if(!typeName)
                return false;

            return typeName.toLowerCase().indexOf(typeNameContains.toLowerCase()) !== -1;
        },
        
        getMetadata: function (typeNameContains) {
            /// <summary>get metatadata for a passed object typeName</summary>
            /// <param name="typeName" type="Object"></param>

            var self = this;
            validateConfiguration(self);
            
            var typeMetadata = ko.utils.arrayFirst(self.allTypeMetadata, function (metadata) {
                return metadata.Type.toLowerCase().indexOf(typeNameContains.toLowerCase()) !== -1;
            });
            
            if (!typeMetadata)
                throw "Type " + typeName + " could not be found in typeMetadata. Is it not defined?";

            return typeMetadata;
        },

        getInstance: function (typeNameContains, settings /*optional*/) {

            settings = $.extend({
                //defaults
                
                //for .NET primitive value types use null instead of the template value if it is default(TValueType)
                //ex a C# int in the template would be '0'. this setting makes it 'null' which is generally
                //what is desired for user input when starting from a template
                nullDefaultValues: true,

                //fields to exclude from the above mapping
                exclude: []

            }, settings);

            var typeMetaData = this.getMetadata(ko.utils.unwrapObservable(typeNameContains));
            var instance = jQuery.extend({}, typeMetaData.Instance);

            var numericToNull = ['short', 'int', 'long', 'float', 'double', 'decimal']; //and DateTime
            
            for (var field in instance) {
                var fieldMetadata = getFieldMetadata(typeMetaData, field);

                if (!fieldMetadata)
                    continue;

                if((instance[field] === 0 && numericToNull.indexOf(fieldMetadata.Type) !== -1) || //numeric
                    instance[field] === "0001-01-01T00:00:00" && //datetime
                    settings.exclude.indexOf(field) == -1)
                    instance[field] = null;
                
            }

            instance[this.typeFieldName] = typeMetaData.Type;
            return instance;
        },
        
        getMappedValidatedInstance: function (typeNameContains) {
            
            //get an instance of a C# (api) Food from the metaDatahelper
            var instance = this.getInstance(typeNameContains);

            //create observables on the instance members
            var observableInstance = ko.mapping.fromJS(instance);

            //apply validation
            this.applyValidation(observableInstance);

            return observableInstance;
        },
        
        getInstanceAndAssign: function (typeNameContains, referenceToAssign, settings /*optional*/) {
            /// <summary>lookup the passed typeName in the configured typeMetadata source and assign it with ko.mapping to the reference</summary>
            var self = this;

            settings = $.extend({
                //pass the settings on "getInstance" above and/or the below
                
                validation: function() { } //additional validation to run right before the type is written as changed
            }, settings);

            validateConfiguration(self);


            var oldtypeMetadata = ko.utils.arrayFirst(self.allTypeMetadata, function (metadata) {
                return self.isType(referenceToAssign, metadata.Type);
            });

            var instance = self.getInstance(typeNameContains, settings);
            
            //first, use ko.mapping to create/update observables fields on the reference in place
            //don't do the type until we are all done since many observables may be dependent on type
            var mappingSettings = {
                ignore: [self.typeFieldName]
            };
            ko.mapping.fromJS(instance, mappingSettings, referenceToAssign);
            
            //second, delete any fields on 'referenceToAssign' that are in the old template but not in the new
            //not strictly necessary but reduces over the wire garbage that would be thrown out by JSON deserializer anyways..
            if(oldtypeMetadata && oldtypeMetadata.Instance) {
                for (var field in oldtypeMetadata.Instance) {
                    if (instance[field] === undefined)
                        delete referenceToAssign[field];
                }
            }
            
            if (ko.validation) { //apply validation right before changing the type
                self.applyValidation(referenceToAssign, instance[self.typeFieldName]);
                settings.validation();
            }

            //finally change the type on the object to the new type and fix mapping to recognize it on ko.mapping.toJSON
            referenceToAssign[self.typeFieldName](instance[self.typeFieldName]);
            referenceToAssign.__ko_mapping__.ignore.splice(self.typeFieldName);
            referenceToAssign.__ko_mapping__.mappedProperties.$type = true;
        },
        
        //REQUIRES knockout.validation plugin in global namespace
        applyValidation: function(object, /*optional*/typeName) {
            /// <summary>recursively apply validation defined in typeMetadata to an entire object graph</summary>
            /// <param name="objectGraph" type="Object">an object graph to apply validation to</param>
            /// <param name="typeName" type="String">treat object as type, don't look for it</param>

            var self = this;

            object = ko.utils.unwrapObservable(object);

            if (!object || !object[this.typeFieldName])
                return; //no type info here...

            var typeMetadata = this.getMetadata(typeName ? typeName : this.getTypeName(object));

            if (!typeMetadata || !typeMetadata.FieldMetadata)
                return;

            for (var fieldName in object) {
                var fieldObservable = object[fieldName];
                
                if (!ko.isObservable(fieldObservable))
                    continue; //can't apply knockout.validation to a non observable field

                parseAndApplyValidations(fieldName, fieldObservable, typeMetadata);

                if(fieldObservable.__valid__) //has ko validation on it
                    fieldObservable.isModified(false); //un-mark fields as being modified if they are since we are now applying validation

                var fieldValue = ko.utils.unwrapObservable(fieldObservable);

                if (!fieldValue)
                    continue; //no more recursion possible
                
                if (typeof fieldValue == "object")
                    self.applyValidation(fieldValue); //continue to recurse down...
                
                if ($.isArray(fieldValue))
                    //loop across array adding validation for each item
                    $.each(fieldValue, function(idx, val) {
                        var obj = ko.utils.unwrapObservable(val);
                        self.applyValidation(obj);
                    });
            }
        },
        
        copy: function (obj, mappingOptions) {
            /// <summary>
            /// Create a copy of an object for 'detaching' from underlying VM in dialogs etc.
            /// call merge() on the result to merge changes back into the original reference
            /// </summary>
            /// <param name="obj" type="Object">the object to copy</param>
            /// <param name="mappingOptions" type="Object">OPTIONAL: ko.mapping settings</param>

            var ctor = obj.constructor;
            var copy = new ctor(ko.mapping.toJS(obj, mappingOptions));
            
            if (ko.validation)
                this.applyValidation(copy);

            
            copy.merge = function (mappingOptions) {
                //call myObj.merge(mappingOptions) at any time to merge changes back into original reference
                ko.mapping.fromJS(ko.mapping.toJS(copy, mappingOptions), mappingOptions, obj);
            };

            return copy;
        }
    };
    
    function validateConfiguration(typeMetadataHelper) {
        
        if (!jQuery || !ko || !ko.mapping)
            throw "typeMetadataHelper doesn't have its required dependencies of jQuery, Knockout.js and knockout.mapping";

        if (!typeMetadataHelper.allTypeMetadata || !jQuery.isArray(typeMetadataHelper.allTypeMetadata))
            throw "typeMetadataHelper must be configured with the source of typeMetadata at Javascript App start. This is usually the result of a C# TypeMetadataHelper.EmitTypeMetadataArray() call";
    }
    
    function getFieldMetadata(typeMetadata, fieldName) {
        return ko.utils.arrayFirst(typeMetadata.FieldMetadata, function (field) {
            return field.Name === fieldName;
        });
    }

    function parseAndApplyValidations(fieldName, fieldObservable, typeMetadata, object) {
        
        var fieldMetadata = getFieldMetadata(typeMetadata, fieldName);

        if (!fieldMetadata)
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
        
        //validations per data type
        switch (fieldMetadata.Type) {
            //ints
            case "short":
                fieldObservable.extend({ required: true, 'short': true });
                break;
            case "short?":
                fieldObservable.extend({ 'short': true });
                break;
            case "int":
                fieldObservable.extend({ required: true, 'int': true });
                break;
            case "int?":
                fieldObservable.extend({ 'int': true });
                break;
            case "long":
                fieldObservable.extend({ required: true, 'long': true });
                break;
            case "long?":
                fieldObservable.extend({ 'long': true });
                break;


                //floats
            case "float":
                fieldObservable.extend({ required: true, number: true });
                break;
            case "float?":
                fieldObservable.extend({ number: true });
                break;
            case "double":
                fieldObservable.extend({ required: true, number: true });
                break;
            case "double?":
                fieldObservable.extend({ number: true });
                break;
            case "decimal":
                fieldObservable.extend({ required: true, number: true });
                break;
            case "decimal?":
                fieldObservable.extend({ number: true });
                break;


                //dates
            case "DateTime":
                fieldObservable.extend({ required: true, date: true });
                break;
            case "DateTime?":
                fieldObservable.extend({ date: true });
                break;
        }
        
        function applyIfHasRule(ruleName, applyRuleFunc) {
            var theRule = ko.utils.arrayFirst(fieldMetadata.Rules, function (rule) {
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
        
        //TODO: this isn't really typeMetadata related and could go somewhere else...
        ko.validation.rules['asyncValidation'] = {
            async: true,
            validator: function (
                val,                //the value of the obserable the extender is applied to, ignored*/
                validationFunc,     //contains a promise returning 'validationFunc' to determine if something is valid or not
                callback            //the func returns obj like: { isValid: false, message: "Invalid!!" } or true/false
            ) {
                validationFunc()
                    .then(function(resp) {
                        callback(resp);
                    })
                    .fail(function() {
                        callback(false);
                    });
            },
            message: 'There is a problem with this field' //default message, shouldn't get used
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