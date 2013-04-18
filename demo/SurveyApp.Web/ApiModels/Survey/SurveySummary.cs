using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SurveyApp.Model.Models;
using System.Data.Entity;

namespace SurveyApp.Web.ApiModels.Survey
{
    //Labels are automatically split on camel casing when using FluentKnockoutHelpers: FirstName => First Name
    public class SurveySummary
    {
        public int SurveyId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Location Location { get; set; }

        [Display(Name = "# of Tech Products")]
        public int NumberOfTechProducts { get; set; }

        [Display(Name = "# of Children")]
        public int NumberOfChildren { get; set; }

        public double OverallHealthyScore { get; set; }
    }

    public static class SurveySummaryExtensions
    {
        public static IEnumerable<SurveySummary> ToSurveySummaries(this IQueryable<Model.Models.Survey> surveys)
        {
            return surveys
                .Include(x => x.TechProducts)
                .Include(x => x.Children)
                .Include(x => x.FavoriteFoods)
                .ToArray()
                .Select(s => new SurveySummary
                {
                    SurveyId = s.SurveyId,
                    LastName = s.LastName,
                    FirstName = s.FirstName,
                    DateOfBirth = s.DateOfBirth.ToShortDateString(),
                    Gender = s.Gender.ToString(),
                    Location = s.HomeLocation,
                    NumberOfTechProducts = s.TechProducts.Count,
                    NumberOfChildren = s.Children.Count,
                    OverallHealthyScore = s.FavoriteFoods.Average(x => x.HealthyScore)
                });
        }
    }
}