namespace SurveyApp.Web.Models
{
    public class KnockoutValidationResult
    {
        public KnockoutValidationResult()
        {
            isValid = true;
        }

        //named to be immediately compatiable with knockout.validation
        public bool isValid { get; set; }
        public string message { get; set; }
    }
}