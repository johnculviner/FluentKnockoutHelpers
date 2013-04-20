using System.Collections.Generic;

namespace SurveyApp.Model.Models
{
    public class Relation
    {
        public string Name { get; set; }
        public List<Relation> Children { get; set; }
    }
}
