using System;

namespace SurveyApp.Model.Models
{
    public class Food
    {
        public Food()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public FoodGroup FoodGroup { get; set; }
        public string Name { get; set; }
        public int HealthyScore { get; set; }
    }
}
