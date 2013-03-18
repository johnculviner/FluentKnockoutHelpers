define(['api/surveyApi'], function (surveyApi) {
    
    return function(survey) {
        var self = this;

        self.survey = ko.observable(survey);

        self.deleteSurvey = function () {
            surveyApi.delete(survey.SurveyId());
            this.modal.close(survey);
        };

        self.close = function() {
            this.modal.close(null);
        };
    };
});