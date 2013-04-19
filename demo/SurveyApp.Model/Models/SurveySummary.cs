using System.ComponentModel.DataAnnotations;
using SurveyApp.Model.DomainModels;

namespace SurveyApp.Model.Models
{
    //Labels are automatically split on camel casing when using FluentKnockoutHelpers: FirstName => First Name
    public class SurveySummary
    {
        public string Id { get; set; }
        
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
}