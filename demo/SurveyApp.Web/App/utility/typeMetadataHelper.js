//for full functionality requires the following loaded in the global namespace
// • knockout.js
// • koockout.mapping plugin
// • jQuery
// • knockout.validation plugin (for applyValidation only)
define(function () {
    return {
        configure: function (typeMetadataource, typeFieldName /*if not using JSON.NET*/) {
            /// <summary>must be defined at JavaScript app start as the result of a C# TypeMetadataHelper.EmitTypeMetadataArray() call</summary>
            /// <param name="typeMetadataource" type="Object"></param>

            this.typeMetadata = typeMetadataource;
            
            if(typeFieldName)
                this.typeFieldName = typeFieldName;

            validateConfiguration(this);

            if (ko && ko.validation)
                wireUpAdditionalValidationRules();
        },
        
        typeMetadata: null, //must be defined, should be a type resulting from a a C# TypeMetadataHelper.EmitTypeMetadataArray() call
        
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
        
        getMetadata: function (typeName) {
            /// <summary>get metatadata for a passed object typeName</summary>
            /// <param name="typeName" type="Object"></param>

            var self = this;
            validateConfiguration(this);
            
            var typeMetadata = ko.utils.arrayFirst(this.typeMetadata, function (metadata) {
                return self.isType(metadata.Instance, typeName);
            });
            
            if (!typeMetadata)
                throw "Type " + typeName + "could not be found in typeMetadata. Is it not defined?";

            return typeMetadata;
        },

        getInstance: function (typeName) {
            return this.getMetadata(typeName).Instance;
        },
        
        createAndAssignType: function (typeName, referenceToAssign) {
            /// <summary>lookup the passed typeName in the configured typeMetadata source and assign it with ko.mapping to the reference</summary>
            /// <param name="typeName" type="Object"></param>
            /// <param name="referenceToAssign" type="Object"></param>

            validateConfiguration(this);

            var self = this;

            var oldtypeMetadata = ko.utils.arrayFirst(self.typeMetadata, function (metadata) {
                return self.isType(referenceToAssign, metadata.TypeName);
            });

            var newtypeMetadata = self.getMetadata(typeName);

            var instance = newtypeMetadata.Instance;
            
            //first, use ko.mapping to create/update observables fields on the reference in place
            //don't do the type until we are all done since many observables may be dependent on type
            var settings = {
                ignore: [self.typeFieldName]
            };
            ko.mapping.fromJS(instance, settings, referenceToAssign);
            
            //second, delete any fields on 'referenceToAssign' that are in the old template but not in the new
            //not strictly necessary but reduces over the wire garbage that would be thrown out by JSON deserializer anyways..
            for (var field in oldtypeMetadata.Instance) {
                if (newtypeMetadata.Instance[field] === undefined)
                    delete referenceToAssign[field];
            }
            
            //finally change the type on the object to the new type and fix mapping to recognize it on ko.mapping.toJSON
            referenceToAssign[self.typeFieldName](instance[self.typeFieldName]);
            referenceToAssign.__ko_mapping__.ignore.splice(self.typeFieldName);
        },
        
        //REQUIRES knockout.validation plugin in global namespace
        applyValidation: function(object) {
            /// <summary>recursively apply validation defined in typeMetadata to an entire object graph</summary>
            /// <param name="objectGraph" type="Object">an object graph to apply validation to</param>

            if (!object || !object[this.typeFieldName])
                return; //no type info here...

            var typeMetadata = this.getMetadata(this.getTypeName(object));

            if (!typeMetadata || !typeMetadata.FieldValidationRules)
                return;

            for (var fieldName in object) {
                parseAndApplyValidations(fieldName, object[fieldName], typeMetadata);
            }
        }
    };
    
    function validateConfiguration(typeMetadataHelper) {
        
        if (!jQuery || !ko || !ko.mapping)
            throw "typeMetadataHelper doesn't have its required dependencies of jQuery, Knockout.js and knockout.mapping";

        if (!typeMetadataHelper.typeMetadata || !jQuery.isArray(typeMetadataHelper.typeMetadata))
            throw "typeMetadataHelper must be configured with the source of typeMetadata at Javascript App start. This is usually the result of a C# TypeMetadataHelper.EmitTypeMetadataArray() call";
    }
    
    function parseAndApplyValidations(fieldName, fieldValue, typeMetadata, object) {
        
        if (!ko.isObservable(fieldValue))
            return; //can't apply knockout.validation to a non observable field

        var fieldValidationRule = ko.utils.arrayFirst(typeMetadata.FieldValidationRules, function (field) {
            return field.FieldName === fieldName;
        });
        
        if (!fieldValidationRule)
            return;


        applyIfHasRule("Required", function (rule) {
            wireUpRule(rule, 'required', true);
        });
        applyIfHasRule("Range", function (rule) {
            wireUpRule(rule, 'min', rule.Minimum);
            wireUpRule(rule, 'max', rule.Maxmium);
        });
        applyIfHasRule("MinLength", function (rule) {
            wireUpRule(rule, 'minLength', rule.Length);
        });
        applyIfHasRule("MaxLength", function (rule) {
            wireUpRule(rule, 'maxLength', rule.Length);
        });
        applyIfHasRule("Regex", function (rule) {
            wireUpRule(rule, 'pattern', rule.Pattern);
        });
        applyIfHasRule("EmailAddress", function (rule) {
            wireUpRule(rule, 'email', true);
        });
        applyIfHasRule("Compare", function (rule) {
            wireUpRule(rule, 'equal', object[rule.OtherField]);
        });

        //TODO datatype rules

        function applyIfHasRule(ruleName, applyRuleFunc) {
            var theRule = ko.utils.arrayFirst(fieldValidationRule.Rules, function (rule) {
                return rule.Name === ruleName;
            });

            if (!theRule)
                return;

            applyRuleFunc(theRule);
        }
        
        function wireUpRule(rule, koValidationRuleName, ruleValue) {
            if (!rule.ErrorMessage)
                fieldValue.extend({ koValidationRuleName: ruleValue }); //use built in validation & message
            else
                //use built in logic but override with custom server specified message
                //by specifiying a "Single-Use Custom Rule" https://github.com/ericmbarnard/Knockout-Validation/wiki/Custom-Validation-Rules
                fieldValue.extend({
                    validation: {
                        validator: function (value) {
                            return ko.validation.rules[koValidationRuleName].validator(value, ruleValue);
                        },
                        message: rule.ErrorMessage,
                    }
                });
        }
    }
    
    //many from here: https://github.com/ericmbarnard/Knockout-Validation/wiki/User-Contributed-Rules
    function wireUpAdditionalValidationRules(){

        ko.validation.rules['creditCard'] = {
            //This rules checks the credit card details 
            //The card number (inferred) as well as the card type (via the card type field) are required 
            //This checks the length and starting digits of the card per the type
            //It also checks the checksum (see http://en.wikipedia.org/wiki/Luhn_algorithm)
            //The card type field must return 'vc' for visa, 'mc' for mastercard, 'ae' for amex
            //This is based on code from here: http://www.rahulsingla.com/blog/2011/08/javascript-implementing-mod-10-validation-(luhn-formula)-for-credit-card-numbers
            //Example:
            //
            //self.cardNumber.extend({ creditCard: self.cardType });
            getValue: function (o) {
                return (typeof o === 'function' ? o() : o);
            },
            validator: function (val, cardTypeField) {
                var self = this;

                var cctype = self.getValue(cardTypeField);
                if (!cctype) return false;
                cctype = cctype.toLowerCase();

                if (val.length < 15) {
                    return (false);
                }
                var match = cctype.match(/[a-zA-Z]{2}/);
                if (!match) {
                    return (false);
                }

                var number = val;
                match = number.match(/[^0-9]/);
                if (match) {
                    return (false);
                }

                var fnMod10 = function (number) {
                    var doubled = [];
                    for (var i = number.length - 2; i >= 0; i = i - 2) {
                        doubled.push(2 * number[i]);
                    }
                    var total = 0;
                    for (var i = ((number.length % 2) == 0 ? 1 : 0) ; i < number.length; i = i + 2) {
                        total += parseInt(number[i]);
                    }
                    for (var i = 0; i < doubled.length; i++) {
                        var num = doubled[i];
                        var digit;
                        while (num != 0) {
                            digit = num % 10;
                            num = parseInt(num / 10);
                            total += digit;
                        }
                    }

                    if (total % 10 == 0) {
                        return (true);
                    } else {
                        return (false);
                    }
                }

                switch (cctype) {
                    case 'vc':
                    case 'mc':
                    case 'ae':
                        //Mod 10 check
                        if (!fnMod10(number)) {
                            return false;
                        }
                        break;
                }
                switch (cctype) {
                    case 'vc':
                        if (number[0] != '4' || (number.length != 13 && number.length != 16)) {
                            return false;
                        }
                        break;
                    case 'mc':
                        if (number[0] != '5' || (number.length != 16)) {
                            return false;
                        }
                        break;

                    case 'ae':
                        if (number[0] != '3' || (number.length != 15)) {
                            return false;
                        }
                        break;

                    default:
                        return false;
                }

                return (true);
            },
            message: 'Card number not valid.'
        };

    }
});