define(['api/surveyApi'], function (surveyApi) {
    
    return function(survey) {
        var self = this;

        self.survey = ko.observable(survey);

        self.deleteSurvey = function () {
            surveyApi.delete(survey.Id());
            this.modal.close(survey);
        };

        self.close = function() {
            this.modal.close(null);
        };
    };
});