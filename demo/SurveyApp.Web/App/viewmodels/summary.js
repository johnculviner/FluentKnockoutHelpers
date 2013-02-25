define(['durandal/app', '../api/survey'], function (app, surveyApi) {
    return function () {
        var self = this;

        self.surveySummaries = null;
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
            Helpers.injectView(app.surveyApiRoot + '/' + survey.SurveyId());
        };

        self.deleteSurvey = function (survey) {
            survey.viewUrl = 'views/delete-modal';
            survey.deleteSurvey = function() {
                surveyApi
                    .delete(survey.SurveyId())
                    .done(function() {
                        if (self.selectedSurvey() == survey)
                            self.selectedSurvey(null);

                        self.surveySummaries.remove(survey);
                    });

                this.modal.close();
            };

            app.showModal(survey);
        };
    };
});