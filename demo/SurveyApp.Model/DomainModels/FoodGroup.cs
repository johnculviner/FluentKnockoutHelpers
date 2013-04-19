using System.Collections.Generic;

namespace SurveyApp.Model.DomainModels
{
    public class FoodGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Food> Foods { get; set; }
    }
}
