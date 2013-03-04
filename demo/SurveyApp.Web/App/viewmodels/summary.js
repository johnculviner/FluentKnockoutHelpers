define(['durandal/app', '../api/survey', 'viewmodels/deleteModal', 'viewmodels/createEditModal'], function (app, surveyApi, DeleteModal, CreateEditModal) {
    return function () {
        var self = this;

        self.surveySummaries = null; //loaded from ajax as ko.observableArray()
        self.loading = ko.observable(true);
        self.selectedSurvey = ko.observable(null);

        //the router's activator calls this function and waits for the .complete jQuery Promise before proceding
        self.activate = function () {
            return surveyApi.getAll()
                        .then(function (surveys) {
                            
                            //use ko.mapping library to convert the array and it's kids to be observable for the summary view
                            self.surveySummaries = ko.mapping.fromJS(surveys);
                            self.loading(false);
                        });
        };

        self.toggleSelected = function (survey) {
            if (self.selectedSurvey() == survey)
                self.selectedSurvey(null);
            else
                self.selectedSurvey(survey);
        };

        self.editSurvey = function (survey) {
            app.showModal(new CreateEditModal(survey));
        };

        self.deleteSurvey = function (survey) {
            app.showModal(new DeleteModal(survey))
                .then(function(deletedSurvey) {
                    if (deletedSurvey !== null)
                        self.surveySummaries.remove(deletedSurvey);
                });
        };
    };
});