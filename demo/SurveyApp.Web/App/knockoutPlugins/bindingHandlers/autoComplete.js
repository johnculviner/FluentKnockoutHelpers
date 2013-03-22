define(function() {
    ko.bindingHandlers.autocomplete = {
        init: function (element, params) {
            $(element).autocomplete(params());
        },
        update: function (element, params) {
            $(element).autocomplete("option", "source", params().source);
        }
    };
});