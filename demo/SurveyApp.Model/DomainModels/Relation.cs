using System.Collections.Generic;

namespace SurveyApp.Model.DomainModels
{
    public class Relation
    {
        public string Name { get; set; }
        public List<Relation> Children { get; set; }
    }
}
