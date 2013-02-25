using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Model.Models
{
    public class FoodGroup
    {
        public int FoodGroupId { get; set; }
        public string Name { get; set; }
        public List<Food> Foods { get; set; }
    }
}
