using SurveyApp.Model.Models;

namespace SurveyApp.Web.Models
{
    public class KnockoutValidationResult
    {
        public KnockoutValidationResult(ValidationResult validationResult)
        {
            isValid = !validationResult.HasError;
            message = validationResult.ErrorMessage;
        }

        //named to be immediately compatiable with knockout.validation
        public bool isValid { get; set; }
        public string message { get; set; }
    }
}