using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Model.Models
{
    public class Relation
    {
        public int RelationId { get; set; }
        public string Name { get; set; }
        public List<Relation> Children { get; set; }
    }
}
