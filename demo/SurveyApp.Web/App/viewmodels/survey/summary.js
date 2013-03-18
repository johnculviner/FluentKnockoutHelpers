define(['durandal/app', 'api/surveyApi', './deleteModal', './createEditModal', './shared/locationInfo'],
    function (app, surveyApi, deleteModal, createEditModal, locationInfo) {

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
                                
                                if (self.surveySummaries().length > 0)
                                    self.toggleSelected(self.surveySummaries()[0]);

                                self.loading(false);
                            });
            };

            //show summary information about a survey when selected
            self.toggleSelected = function (surveySummary) {
                self.selectedSurvey(surveySummary);
            };

            //TODO: navigate to a page to edit the survey
            self.editSurvey = function (surveySummary) {
                app.showModal(new createEditModal(surveySummary));
            };

            //open a modal requesting confirmation to delete the indicated modal
            self.deleteSurvey = function (surveySummary) {
                app.showModal(new deleteModal(surveySummary))
                    .then(function (deletedSurvey) {
                        if (deletedSurvey !== null)
                            self.surveySummaries.remove(deletedSurvey);
                    });
            };

            self.locationInfo = locationInfo;
        };
    });