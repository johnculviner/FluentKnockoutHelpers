namespace SurveyApp.Model.DomainModels
{
    public class Food
    {
        public string Id { get; set; }
        public FoodGroup FoodGroup { get; set; }
        public string Name { get; set; }
        public int HealthyScore { get; set; }
    }
}
