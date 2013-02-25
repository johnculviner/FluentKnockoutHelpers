using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Model.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public FoodGroup FoodGroup { get; set; }
        public string Name { get; set; }
        public int HealthyScore { get; set; }
    }
}
