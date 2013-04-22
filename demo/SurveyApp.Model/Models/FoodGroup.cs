using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Model.Models
{
    public class FoodGroup
    {
        public FoodGroup()
        {
            Foods = new List<Food>();
        }

        public string Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public List<Food> Foods { get; set; }
    }
}
