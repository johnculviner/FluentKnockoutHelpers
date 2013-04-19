using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Linq;
using SurveyApp.Model.DomainModels;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Services
{
    public interface ISurveyService
    {
        IEnumerable<SurveySummary> GetSummaries();
        Survey Get(string id);
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
    }
}
