using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SurveyApp.Model.Models;
using System.Data.Entity;

namespace SurveyApp.Web.ApiModels.Survey
{
    public class SurveySummary
    {
        public int SurveyId { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "# of Tech Products")]
        public int NumberOfTechProducts { get; set; }

        [Display(Name = "# of Children")]
        public int NumberOfChildren { get; set; }

        [Display(Name = "Overall Healthy Score")]
        public double OverallHealthyScore { get; set; }
    }

    public static class SurveySummaryExtensions
    {
        public static IEnumerable<SurveySummary> ToSurveySummaries(this IQueryable<Model.Models.Survey> surveys, IQueryable<ZipCodeLocation> zipCodes)
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
                    State = zipCodes.Where(z => z.ZipCode == s.ZipCode).Select(z => z.State).FirstOrDefault() ?? "Unknown",
                    NumberOfTechProducts = s.TechProducts.Count,
                    NumberOfChildren = s.Children.Count,
                    OverallHealthyScore = s.FavoriteFoods.Average(x => x.HealthyScore)
                });
        }
    }
}