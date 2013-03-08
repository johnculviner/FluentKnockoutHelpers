define(['durandal/app', 'api/surveyApi', './deleteModal', './createEditModal'],
    function (app, surveyApi, DeleteModal, CreateEditModal) {

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

        self.toggleSelected = function (surveySummary) {
            if (self.selectedSurvey() == surveySummary)
                self.selectedSurvey(null);
            else
                self.selectedSurvey(surveySummary);
        };

        self.editSurvey = function (surveySummary) {
            app.showModal(new CreateEditModal(surveySummary));
        };

        self.deleteSurvey = function (surveySummary) {
            app.showModal(new DeleteModal(surveySummary))
                .then(function(deletedSurvey) {
                    if (deletedSurvey !== null)
                        self.surveySummaries.remove(deletedSurvey);
                });
        };
    };
});