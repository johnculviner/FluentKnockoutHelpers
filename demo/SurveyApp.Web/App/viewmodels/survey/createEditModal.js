define(['api/surveyApi'], function (surveyApi, zipCodeLocationApi) {

    var CreateEditModal = function (surveySummary) {

        var self = this;

        self.survey = ko.observable();

        //the router's activator calls this function and waits for the .complete jQuery Promise before proceding
        self.activate = function () {
            return surveyApi.get(surveySummary.SurveyId())
                        .then(function (fullSurvey) {

                            //use ko.mapping library to convert the entire survey to an observable
                            self.survey(ko.mapping.fromJS(fullSurvey));
                        });
        };


        




        self.close = function () {
            this.modal.close(null);
        };
    };

    return CreateEditModal;
});