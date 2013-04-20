using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Linq;
using SurveyApp.Model.Models;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Services
{
    public interface ISurveyService
    {
        IEnumerable<SurveySummary> GetSummaries();
        Survey Get(string id);
        void Save(Survey survey);
        ValidationResult ValidatePersonIdNumberUnique(ValidatePersonIdNumberUnique dto);
        void Delete(string surveyId);
    }

    public class SurveyService : ISurveyService
    {
        private readonly IDocumentSession _documentSession;

        public SurveyService(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public IEnumerable<SurveySummary> GetSummaries()
        {
            return _documentSession.Query<Survey>()
                    .ToArray()
                    .Select(s => new SurveySummary
                    {
                        Id = s.Id,
                        LastName = s.LastName,
                        FirstName = s.FirstName,
                        DateOfBirth = s.DateOfBirth.ToShortDateString(),
                        Gender = s.Gender.ToString(),
                        Location = s.HomeLocation,
                        NumberOfTechProducts = s.TechProducts.Count,
                        NumberOfChildren = s.Children.Count
                        //OverallHealthyScore = s.FavoriteFoods.Average(x => x.HealthyScore)
                    }).ToList();
        }

        public Survey Get(string id)
        {
            return _documentSession.Load<Survey>(id);
        }

        public void Save(Survey survey)
        {
            _documentSession.Store(survey);
        }

        public void Delete(string surveyId)
        {
            _documentSession.Advanced.Defer(new DeleteCommandData { Key = surveyId });
        }

        public ValidationResult ValidatePersonIdNumberUnique(ValidatePersonIdNumberUnique dto)
        {
            var existingSurvey = _documentSession.Query<Survey>()
                .FirstOrDefault(s => s.Id != dto.CurrentSurveyId && s.PersonIdNumber == dto.PersonIdNumber);

            if (existingSurvey != null)
                return new ValidationResult
                {
                    HasError = true,
                    ErrorMessage = string.Format("{0} {1} already has this ID number!", existingSurvey.FirstName, existingSurvey.LastName)
                };

            return new ValidationResult(); //no problems...
        }
    }
}
