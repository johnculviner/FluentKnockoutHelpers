using System.Collections.Generic;

namespace SurveyApp.Model.Models
{
    public class FoodGroup
    {
        public FoodGroup()
        {
            Foods = new List<Food>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<Food> Foods { get; set; }
    }
}
