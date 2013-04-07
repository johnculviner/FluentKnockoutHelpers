define(function () {
    //Wires up validations that come from WebAPI if the ClientValidationJsonConverter from FluentKnockoutHelpers is added as a converter
    //REQUIRES: ko.mapping, kockout.validation
    return {
        configure: function() {
            //configure ko.mappings to globally recognise __validation_rules__ on incoming JSON that is being mapped
            //and automatically wire up the rules with knockout.validation
            

        }
    };
});