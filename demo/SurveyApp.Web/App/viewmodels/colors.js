define(['api/colorsApi', 'durandal/plugins/router', 'durandal/app', 'utility/typeMetadataHelper', 'utility/uuid'], function (colorsApi, router, app, tmh, uuid) {
    return function() {
        var self = this;

        self.colors = null;

        //special durandal function, doesn't render view until
        //promise returns
        self.activate = function() {
            return colorsApi.getAll().then(function (colors) {
                //creates observables for all properties on colors for knockout
                self.colors = ko.mapping.fromJS(colors);
            });
        };

        self.save = function() {
            colorsApi.post(ko.mapping.toJS(self.colors))
                .then(function() {
                    router.navigateTo('#/surveys');
                });
        };

        self.addColor = function () {
            var newColor = tmh.getMappedValidatedInstance("color,");
            newColor.Id(uuid.v4());

            self.colors.push(newColor);
        };

        self.removeColor = function(color) {

            var yes = "Yes";
            var no = "No";

            app.showMessage(
                "Are you sure you want to remove " + "?",
                "Remove color",
                [yes, no])
                .then(function(resp) {
                    if(resp == yes)
                        self.colors.remove(color);
                });

        };
    };
});